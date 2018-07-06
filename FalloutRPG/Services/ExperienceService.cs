using Discord;
using Discord.WebSocket;
using FalloutRPG.Constants;
using FalloutRPG.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace FalloutRPG.Services
{
    public class ExperienceService
    {
        private Dictionary<ulong, Timer> CooldownTimers;
        private List<ulong> ExperienceEnabledChannels;

        private const int DEFAULT_EXP_GAIN = 100;
        private const int DEFAULT_EXP_RANGE_FROM = 50;
        private const int DEFAULT_EXP_RANGE_TO = 150;
        private const int COOLDOWN_INTERVAL = 30000;

        private readonly CharacterService _charService;
        private readonly SkillsService _skillsService;
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _config;

        public ExperienceService(CharacterService charService, SkillsService skillsService, DiscordSocketClient client, IConfiguration config)
        {
            _charService = charService;
            _skillsService = skillsService;
            _client = client;
            _config = config;

            CooldownTimers = new Dictionary<ulong, Timer>();
            LoadExperienceEnabledChannels();
        }

        /// <summary>
        /// Gives experience to a character.
        /// </summary>
        public async Task<bool> GiveExperienceAsync(Character character, int experience = DEFAULT_EXP_GAIN)
        {
            if (character == null) return false;
            if (CooldownTimers.ContainsKey(character.DiscordId)) return false;

            var levelUp = false;

            if (WillLevelUp(character, experience))
            {
                await OnLevelUpAsync(character);
                levelUp = true;
            }

            character.Experience += experience;
            await _charService.SaveCharacterAsync(character);

            AddToCooldown(character.DiscordId);
            return levelUp;
        }

        /// <summary>
        /// Gets a random amount of experience to give
        /// between a range of two numbers.
        /// </summary>
        public int GetRandomExperience(
            int rangeFrom = DEFAULT_EXP_RANGE_FROM,
            int rangeTo = DEFAULT_EXP_RANGE_TO)
        {
            var random = new Random();
            return random.Next(rangeFrom, rangeTo);
        }

        /// <summary>
        /// Adds a user's Discord ID to the cooldowns.
        /// </summary>
        public void AddToCooldown(ulong discordId)
        {
            var timer = new Timer();
            timer.Elapsed += (sender, e) => OnCooldownElapsed(sender, e, discordId);
            timer.Interval = COOLDOWN_INTERVAL;
            timer.Enabled = true;

            CooldownTimers.Add(discordId, timer);
        }

        /// <summary>
        /// Calculate the experience required for a level.
        /// </summary>
        public int CalculateExperienceForLevel(int level)
        {
            if (level < 1 || level > 1000) return -1;
            return (level * (level - 1) / 2) * 1000;
        }

        /// <summary>
        /// Calculates experience between the beginning of
        /// one level and the next.
        /// </summary>
        public int CalculateExperienceToNextLevel(int level)
        {
            if (level < 1 || level > 1000) return -1;
            return 50 + (150 * level);
        }

        /// <summary>
        /// Calculates the remaining experience required
        /// to get to the next level.
        /// </summary>
        public int CalculateRemainingExperienceToNextLevel(int experience)
        {
            var nextLevel = (CalculateLevelForExperience(experience) + 1);
            var nextLevelExp = CalculateExperienceForLevel(nextLevel);
            return (nextLevelExp - experience);
        }

        /// <summary>
        /// Calculates the level depending on the experience.
        /// </summary>
        public int CalculateLevelForExperience(int experience)
        {
            if (experience == 0) return 1;
            return Convert.ToInt32((Math.Sqrt(experience + 125) / (10 * Math.Sqrt(5))));
        }

        /// <summary>
        /// Checks if the input Channel ID is an experience
        /// enabled channel.
        /// </summary>
        public bool IsInExperienceEnabledChannel(ulong channelId)
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
                    .ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("You have not specified any experience enabled channels in Config.json");
            }
        }

        /// <summary>
        /// Checks if adding the experience will result in a level up.
        /// </summary>
        private bool WillLevelUp(Character character, int expToAdd)
        {
            int nextLevelExp = CalculateExperienceForLevel(CalculateLevelForExperience(character.Experience) + 1);

            return (character.Experience + expToAdd) >= nextLevelExp;
        }

        /// <summary>
        /// Called when a character levels up.
        /// </summary>
        private async Task OnLevelUpAsync(Character character)
        {
            var user = _client.GetUser(character.DiscordId);

            _skillsService.GiveSkillPoints(character);

            await user.SendMessageAsync(string.Format(Messages.SKILLS_LEVEL_UP, user.Mention, character.SkillPoints));
        }

        /// <summary>
        /// Called when a cooldown has finished.
        /// </summary>
        private void OnCooldownElapsed(object sender, ElapsedEventArgs e, ulong discordId)
        {
            var timer = CooldownTimers[discordId];
            timer.Enabled = false;
            timer.Dispose();

            CooldownTimers.Remove(discordId);
        }
    }
}
