using Discord.Commands;
using Discord.WebSocket;
using FalloutRPG.Constants;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly ExperienceService _expService;
        private readonly CharacterService _charService;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _config;

        public CommandHandler(
            DiscordSocketClient client,
            CommandService commands,
            ExperienceService expService,
            CharacterService charService,
            IServiceProvider services,
            IConfiguration config)
        {
            _client = client;
            _commands = commands;
            _expService = expService;
            _charService = charService;
            _services = services;
            _config = config;
        }

        /// <summary>
        /// Installs the commands and subscribes to MessageReceived event.
        /// </summary>
        public async Task InstallCommandsAsync()
        {
            await _commands.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: _services);
            _client.MessageReceived += HandleCommandAsync;
        }

        /// <summary>
        /// Handles incoming commands if it begins with specified prefix.
        /// If there is no prefix, it will process experience.
        /// </summary>
        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            SocketCommandContext context = null;

            var message = messageParam as SocketUserMessage;
            if (message == null || message.Author.IsBot) return;

            int argPos = 0;

            if (!(message.HasStringPrefix(_config["prefix"], ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
            {
                await ProcessExperienceAsync(new SocketCommandContext(_client, message));
                return;
            }

            context = new SocketCommandContext(_client, message);

            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }

        /// <summary>
        /// Processes experience to give if channel is an experience
        /// enabled channel.
        /// </summary>
        private async Task ProcessExperienceAsync(SocketCommandContext context)
        {
            if (!_expService.IsInExperienceEnabledChannel(context.Channel.Id)) return;

            var userInfo = context.User;
            var character = await _charService.GetCharacterAsync(userInfo.Id);

            if (character == null) return;

            var expToGive = _expService.GetRandomExperience();

            if (await _expService.GiveExperienceAsync(character, expToGive))
            {
                var level = _expService.CalculateLevelForExperience(character.Experience);
                await context.Channel.SendMessageAsync(
                    string.Format(Messages.EXP_LEVEL_UP, userInfo.Mention, level));
            }
        }
    }
}
