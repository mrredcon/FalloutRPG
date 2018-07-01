using FalloutRPG.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace FalloutRPG.Services
{
    public class ExperienceService
    {
        private Dictionary<ulong, Timer> CooldownTimers;

        private const int DEFAULT_EXP_GAIN = 100;
        private const int COOLDOWN_INTERVAL = 30000;

        private readonly CharacterService _charService;

        public ExperienceService(CharacterService charService)
        {
            CooldownTimers = new Dictionary<ulong, Timer>();

            _charService = charService;
        }

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

        public async Task<bool> GiveRandomExperienceAsync(Character character, int rangeFrom, int rangeTo)
        {
            if (character == null) return false;
            if (CooldownTimers.ContainsKey(character.DiscordId)) return false;

            var random = new Random();
            var experience = random.Next(rangeFrom, rangeTo);
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

        public void AddToCooldown(ulong discordId)
        {
            var timer = new Timer();
            timer.Elapsed += (sender, e) => OnCooldownElapsed(sender, e, discordId);
            timer.Interval = COOLDOWN_INTERVAL;
            timer.Enabled = true;

            CooldownTimers.Add(discordId, timer);
            Console.WriteLine($"Added {discordId} to cooldowns.");
        }

        private void OnCooldownElapsed(object sender, ElapsedEventArgs e, ulong discordId)
        {
            var timer = CooldownTimers[discordId];
            timer.Enabled = false;
            timer.Dispose();

            CooldownTimers.Remove(discordId);
            Console.WriteLine($"Removed {discordId} from cooldowns.");
        }

        public int CalculateExperienceForLevel(int level)
        {
            if (level < 1 || level > 1000) return -1;
            return (level * (level - 1) / 2) * 1000;
        }

        public int CalculateExperienceToNextLevel(int level)
        {
            if (level < 1 || level > 1000) return -1;
            return 50 + (150 * level);
        }

        public int CalculateRemainingExperienceToNextLevel(int experience)
        {
            var nextLevel = (CalculateLevelForExperience(experience) + 1);
            var nextLevelExp = CalculateExperienceForLevel(nextLevel);
            return (nextLevelExp - experience);
        }

        public int CalculateLevelForExperience(int experience)
        {
            if (experience == 0) return 1;
            return Convert.ToInt32((Math.Sqrt(experience + 125) / (10 * Math.Sqrt(5))));
        }

        private bool WillLevelUp(Character character, int expToAdd)
        {
            int nextLevelExp = CalculateExperienceForLevel(CalculateLevelForExperience(character.Experience) + 1);

            if ((character.Experience + expToAdd) >= nextLevelExp)
                return true;

            return false;
        }

        private async Task OnLevelUpAsync(Character character)
        {
            // Give points to spend

            await _charService.SaveCharacterAsync(character);
        }
    }
}
