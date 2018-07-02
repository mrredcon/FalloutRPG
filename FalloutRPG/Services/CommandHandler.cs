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
        private ulong[] ExperienceEnabledChannels;

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
            
            LoadExperienceEnabledChannels();
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
            if (!IsInExperienceChannel(context.Channel.Id)) return;

            var userInfo = context.User;
            var character = _charService.GetCharacter(userInfo.Id);

            if (character == null) return;

            if (await _expService.GiveRandomExperienceAsync(character, 50, 150))
            {
                var level = _expService.CalculateLevelForExperience(character.Experience);
                await context.Channel.SendMessageAsync(
                    string.Format(Messages.EXP_LEVEL_UP, userInfo.Mention, level));
            }
        }

        /// <summary>
        /// Checks if the input Channel ID is an experience
        /// enabled channel.
        /// </summary>
        private bool IsInExperienceChannel(ulong channelId)
        {
            foreach (var channel in ExperienceEnabledChannels)
                if (channelId == channel)
                    return true;

            return false;
        }

        /// <summary>
        /// Loads the experience enabled channels from the
        /// configuration file.
        /// </summary>
        private void LoadExperienceEnabledChannels()
        {
            try
            {
                ExperienceEnabledChannels = _config
                    .GetSection("roleplay:exp-channels")
                    .GetChildren()
                    .Select(x => UInt64.Parse(x.Value))
                    .ToArray();
            }
            catch (Exception)
            {
                Console.WriteLine("You have not specified any experience enabled channels in Config.json");
            }
        }
    }
}
