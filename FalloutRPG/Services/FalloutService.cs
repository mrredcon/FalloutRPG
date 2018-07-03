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
            // total up the given SPECIAL stats
            int total = special.Strength + special.Perception + special.Endurance + special.Charisma + special.Intelligence +
                special.Agility + special.Luck;

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
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Calculate all 13 Skills' initial values.
        /// Will not add a CharacterId to the returned skill sheet.
        /// </summary>
        /// <param name="special">The character's SPECIAL to calculate Skills for.</param>
        /// <returns>A SkillSheet with the appropriate Skill values for the SPECIAL given without a CharacterId set.</returns>
        /// <remarks>This method will not add a CharacterId to this SkillSheet.</remarks>
        public SkillSheet CalculateInitialSkills(Special special)
        {
            SkillSheet skills = new SkillSheet
            {
                Barter = CalculateSkill(special.Charisma, special.Luck),
                EnergyWeapons = CalculateSkill(special.Perception, special.Luck),
                Explosives = CalculateSkill(special.Perception, special.Luck),
                Guns = CalculateSkill(special.Agility, special.Luck),
                Lockpick = CalculateSkill(special.Perception, special.Luck),
                Medicine = CalculateSkill(special.Intelligence, special.Luck),
                MeleeWeapons = CalculateSkill(special.Strength, special.Luck),
                Repair = CalculateSkill(special.Intelligence, special.Luck),
                Science = CalculateSkill(special.Intelligence, special.Luck),
                Sneak = CalculateSkill(special.Agility, special.Luck),
                Speech = CalculateSkill(special.Charisma, special.Luck),
                Survival = CalculateSkill(special.Endurance, special.Luck),
                Unarmed = CalculateSkill(special.Endurance, special.Luck)
            };

            return skills;
        }
        /// <summary>
        /// Calculate all 13 Skills' initial values.
        /// </summary>
        /// <param name="special">The character's SPECIAL to calculate Skills for.</param>
        /// <param name="character">The character to add Skills to.</param>
        /// <returns>A SkillSheet with the appropriate Skill values for the SPECIAL given.</returns>
        public SkillSheet CalculateInitialSkills(Special special, Character character)
        {
            var skills = CalculateInitialSkills(special);
            skills.CharacterId = character.Id;
            return skills;
        }
        /// <summary>
        /// Calculate the initial value of a skill based on its relevant SPECIAL stat, and Luck (New Vegas)
        /// </summary>
        /// <param name="skillSpecialStat">The value of the SPECIAL stat associated with the Skill</param>
        /// <param name="luck">Character's Luck stat</param>
        /// <returns>The integer value of the Skill</returns>
        public int CalculateSkill(int skillSpecialStat, int luck)
        {
            return (int)(2 + (2 * skillSpecialStat) + Math.Round(luck / 2.0));
        }
        public SkillSheet TagSkills(SkillSheet skillSheet, string tag1, string tag2, string tag3)
        {
            int taggedSkills = 0;

            tag1 = tag1.ToLower(); tag2 = tag2.ToLower(); tag3 = tag3.ToLower();

            if (tag1.Equals(tag2) || tag1.Equals(tag3) || tag2.Equals(tag3))
                throw new ArgumentException(Constants.Messages.EXC_SKILLS_TAGSNOTUNIQUE);

            foreach (var prop in typeof(SkillSheet).GetProperties())
            {
                if (prop.Name.Equals("characterid"))
                    continue;

                if (prop.Name.ToLower().Equals(tag1) || prop.Name.ToLower().Equals(tag2) || prop.Name.ToLower().Equals(tag3))
                {
                    prop.SetValue(skillSheet, (int)prop.GetValue(skillSheet) + 15);
                    taggedSkills++;
                }
            }

            if (taggedSkills != 3)
                throw new ArgumentException(Constants.Messages.EXC_SKILLS_TAGSINVALID);

            return skillSheet;
        }
    }
}