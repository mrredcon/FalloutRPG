using Discord.Commands;
using Discord.WebSocket;
using FalloutRPG.Constants;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
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

        public async Task InstallCommandsAsync()
        {
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
            _client.MessageReceived += HandleCommandAsync;
        }

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

        private async Task ProcessExperienceAsync(SocketCommandContext context)
        {
            if (!IsInExperienceChannel(context.Channel.Id)) return;

            var userInfo = context.User;
            var character = _charService.GetCharacter(userInfo.Id);

            if (character == null) return;

            if (await _expService.GiveRandomExperienceAsync(character, 50, 150))
            {
                var level = _expService.CalculateLevelForExperience(character.Experience);
                await context.Channel.SendMessageAsync(string.Format(Messages.EXP_LEVEL_UP, userInfo.Mention, level));
            }
        }

        private bool IsInExperienceChannel(ulong channelId)
        {
            var channelsArray = _config
                .GetSection("roleplay:exp-channels")
                .GetChildren()
                .Select(x => x.Value)
                .ToArray();

            foreach (var channel in channelsArray)
                if (channelId == UInt64.Parse(channel))
                    return true;

            return false;
        }
    }
}
