using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class StartupService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IConfiguration _config;

        public StartupService(DiscordSocketClient client, CommandService commands, IConfiguration config)
        {
            _client = client;
            _commands = commands;
            _config = config;
        }

        /// <summary>
        /// Loads the Discord Bot token from the configuration
        /// file and attempts to login to Discord and start up.
        /// </summary>
        public async Task StartAsync()
        {
            string discordToken = _config["tokens:discord"];
            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception("Please enter a valid bot token in the Config.json file.");

            await _client.LoginAsync(TokenType.Bot, discordToken);
            await _client.StartAsync();
        }
    }
}
