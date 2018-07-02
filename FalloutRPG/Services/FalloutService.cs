using Discord;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class FalloutService
    {
        private const int _specialPoints = 40;
        private readonly IRepository<Special> _special;

        public FalloutService(IRepository<Special> special)
        {
            _special = special;
        }
        /// <summary>
        /// Attempts to parse the given string into a Special
        /// </summary>
        /// <param name="specialToParse">7 character string 1-9 + X</param>
        /// <returns>A Special if successful, or null if parsing failed.</returns>
        public Special ParseSpecialString(string specialToParse)
        {
            if (specialToParse.Length != 7)
                return null;

            int[] statArray = new int[7];

            int temp = 0;
            for (int stat = 0; stat < specialToParse.Length; stat++)
            {
                if (int.TryParse(specialToParse[stat].ToString(), out temp))
                    statArray[stat] = temp;
                else if (specialToParse[stat] == 'x' || specialToParse[stat] == 'X')
                    statArray[stat] = 10;
                else
                    return null;
            }

            Special special = new Special
            {
                Strength = statArray[0],
                Perception = statArray[1],
                Endurance = statArray[2],
                Charisma = statArray[3],
                Intelligence = statArray[4],
                Agility = statArray[5],
                Luck = statArray[6]
            };

            return special;
        }
        /// <summary>
        /// Checks each SPECIAL stat is at least 1 (or optionally 1-10), and if it sums to _specialPoints.
        /// </summary>
        /// <param name="special">Special to check</param>
        /// <param name="newChar">When set to true, only stats 1-10 are allowed, set to false stats must be at least 1.</param>
        /// <returns>A boolean stating whether the given Special is valid or not.</returns>
        public bool IsValidSpecial(Special special, bool newChar = false)
        {
            int total = 0;

            // total up the given SPECIAL stats
            foreach (var stat in typeof(Special).GetProperties())
                total += (int)stat.GetValue(special);

            if (total != _specialPoints)
                return false;

            // New characters can't have a Special stat above 10 or below 1
            if (newChar)
            {
                if ((special.Strength >= 1 && special.Strength <= 10) &&
                    (special.Perception >= 1 && special.Perception <= 10) &&
                    (special.Endurance >= 1 && special.Endurance <= 10) &&
                    (special.Charisma >= 1 && special.Charisma <= 10) &&
                    (special.Intelligence >= 1 && special.Intelligence <= 10) &&
                    (special.Agility >= 1 && special.Agility <= 10) &&
                    (special.Luck >= 1 && special.Luck <= 10))
                    return true;
                return false;
            }
            // But existing characters can go above 10 with the help of items (or perks)
            else
            {
                if (special.Strength >= 1 &&
                    special.Perception >= 1 &&
                    special.Endurance >= 1 &&
                    special.Charisma >= 1 &&
                    special.Intelligence >= 1 &&
                    special.Agility >= 1 &&
                    special.Luck >= 1)
                    return true;
                return false;
            }
        }

        public async Task SaveSpecial(Character character, Special specialToSave)
        {
            character.Special = specialToSave;
            await _special.SaveAsync(specialToSave);
        }
    }
}