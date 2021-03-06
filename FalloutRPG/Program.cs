﻿using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using FalloutRPG.Data;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Models;
using FalloutRPG.Services;
using FalloutRPG.Services.Casino;
using FalloutRPG.Services.Roleplay;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FalloutRPG
{
    public class Program
    {
        private IConfiguration config;

        /// <summary>
        /// The entry point of the program.
        /// </summary>
        public static void Main(string[] args)
                => new Program().MainAsync().GetAwaiter().GetResult();

        /// <summary>
        /// Initializes the services from the service provider.
        /// </summary>
        /// <remarks>
        /// await Task.Delay(-1) is required or else the program
        /// will end.
        /// </remarks>
        public async Task MainAsync()
        {
            config = BuildConfig();
            CheckConnectionString();

            var services = BuildServiceProvider();

            services.GetRequiredService<LogService>();
            await services.GetRequiredService<CommandHandler>().InstallCommandsAsync();
            await services.GetRequiredService<StartupService>().StartAsync();

            await Task.Delay(-1);
        }

        /// <summary>
        /// Builds the service provider for dependency injection.
        /// </summary>
        private IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async
            }))
            .AddSingleton(config)
            .AddSingleton<CommandHandler>()
            .AddSingleton<LogService>()
            .AddSingleton<StartupService>()
            .AddSingleton<HelpService>()

            // Roleplay
            .AddSingleton<RollService>()
            .AddSingleton<SkillsService>()
            .AddSingleton<SpecialService>()
            .AddSingleton<StartupService>()
            .AddSingleton<CharacterService>()
            .AddSingleton<ExperienceService>()

            // Casino
            .AddSingleton<GamblingService>()
            .AddSingleton<CrapsService>()

            // Addons
            .AddSingleton<InteractiveService>()

            // Database
            .AddDbContext<RpgContext>(options =>
                options.UseSqlServer(config["sqlserver-connection-string"]))
            .AddTransient<IRepository<Character>, EfRepository<Character>>()
            .AddTransient<IRepository<SkillSheet>, EfRepository<SkillSheet>>()
            .AddTransient<IRepository<Special>, EfRepository<Special>>()
            .BuildServiceProvider();

        /// <summary>
        /// Builds the configuration from the Config.json file.
        /// </summary>
        private IConfiguration BuildConfig() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Config.json")
            .Build();

        /// <summary>
        /// Sets the SQL Server connection string variable if it's valid.
        /// </summary>
        private bool CheckConnectionString()
        {
            var connectionString = config["sqlserver-connection-string"];
            if (!string.IsNullOrEmpty(connectionString)) return true;

            Console.WriteLine("You have an invalid SQL Server connection string set in Config.json");
            return false;
        }
    }
}