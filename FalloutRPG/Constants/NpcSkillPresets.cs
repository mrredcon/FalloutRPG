using FalloutRPG.Models;
using FalloutRPG.Services.Roleplay;
using System;
using System.Reflection;

namespace FalloutRPG.Constants
{
    public static class NpcSkillPresets
    {
        private static Random rand = new Random();

        public static SkillSheet Raider(Character character) => GetRandomInitialSkills(character, TagType.Violent, TagType.Violent, TagType.Violent);
        public static SkillSheet RaiderVeteran(Character character) => GetRandomInitialSkills(character, TagType.Violent, TagType.Violent, TagType.Handy);
        public static SkillSheet Mercenary(Character character)
        {
            var skills = GetRandomInitialSkills(character, TagType.Violent, TagType.Handy, TagType.Diplomatic);
            AddPoints(skills, "Guns", 15);
            return skills;
        }
        public static SkillSheet Spy(Character character)
        {
            var skills = GetRandomInitialSkills(character, TagType.Violent, TagType.Handy, TagType.Diplomatic);
            AddPoints(skills, "Guns", 15);
            AddPoints(skills, "Stealth", 20);
            AddPoints(skills, "Lockpick", 15);
            AddPoints(skills, "Speech", 15);
            return skills;
        }
        public static SkillSheet Merchant(Character character)
        {
            var skills = GetRandomInitialSkills(character, TagType.Diplomatic, TagType.Handy, TagType.Diplomatic);
            AddPoints(skills, "Speech", 15);
            AddPoints(skills, "Barter", 20);
            return skills;
        }
        public static SkillSheet Protectron(Character character)
        {
            var skills = GetRandomInitialSkills(character, TagType.Handy, TagType.Handy, TagType.Diplomatic);

            skills.Guns = 0;
            skills.Lockpick = 0;

            AddPoints(skills, "Sneak", -15);

            return skills;
        }
        public static SkillSheet Assaultron(Character character)
        {
            var skills = SkillsService.GetInitialSkills(character);

            skills.Guns = 0;
            skills.Medicine = 0;
            skills.Repair = 0;

            AddPoints(skills, "EnergyWeapons", 30);
            // do you count the claws as melee weapons??
            AddPoints(skills, "MeleeWeapons", 30);
            AddPoints(skills, "Unarmed", 30);

            return skills;
        }
        public static SkillSheet Eyebot(Character character)
        {
            var skills = SkillsService.GetInitialSkills(character);

            skills.Guns = 0;
            skills.Lockpick = 0;
            skills.Medicine = 0;

            AddPoints(skills, "EnergyWeapons", 20);
            AddPoints(skills, "Unarmed", 15);

            return skills;
        }
        public static SkillSheet MisterHandy(Character character)
        {
            var skills = GetRandomInitialSkills(character, TagType.Diplomatic, TagType.Handy, TagType.Diplomatic);

            skills.Guns = 0;
            skills.Lockpick = 0;
            skills.Medicine = 0;

            AddPoints(skills, "EnergyWeapons", 20);
            AddPoints(skills, "MeleeWeapons", 15);

            return skills;
        }
        public static SkillSheet MisterGutsy(Character character)
        {
            var skills = GetRandomInitialSkills(character, TagType.Violent, TagType.Violent, TagType.Handy);

            skills.Lockpick = 0;
            skills.Medicine = 0;
            skills.Repair = 0;

            AddPoints(skills, "MeleeWeapons", 10);

            return skills;
        }
        public static SkillSheet Robobrain(Character character)
        {
            var skills = GetRandomInitialSkills(character, TagType.Handy, TagType.Diplomatic, TagType.Handy);

            skills.Barter = 0;
            skills.Lockpick = 0;
            skills.Medicine = 0;
            skills.Repair = 0;

            AddPoints(skills, "EnergyWeapons", 25);

            return skills;
        }
        public static SkillSheet SentryBot(Character character)
        {
            var skills = SkillsService.GetInitialSkills(character);

            skills.Guns = 80;
            skills.EnergyWeapons = 95;
            skills.MeleeWeapons = 50;
            skills.Lockpick = 0;
            skills.Medicine = 0;
            skills.Repair = 0;

            return skills;
        }
        public static SkillSheet Securitron(Character character)
        {
            var skills = SkillsService.GetInitialSkills(character);

            skills.Guns = 50;
            skills.EnergyWeapons = 50;
            skills.Explosives = 35;
            skills.MeleeWeapons = 35;
            skills.Lockpick = 0;
            skills.Medicine = 0;
            skills.Repair = 0;
            skills.Unarmed = 35;

            return skills;
        }
        public static SkillSheet SuperMutant(Character character)
        {
            var skills = GetRandomInitialSkills(character, TagType.Violent, TagType.Violent, TagType.Handy);

            skills.Lockpick = 0;

            AddPoints(skills, "Repair", -15);

            return skills;
        }
        public static SkillSheet FeralGhoul(Character character) => new SkillSheet
        {
            Unarmed = 50
        };

        private static void AddPoints(SkillSheet sheet, string skillName, int amount)
        {
            var skill = typeof(SkillSheet).GetProperty(skillName);

            if (skill == null)
                throw new ArgumentException("Couldn't parse given skill.", "skillName");

            int skillAmount = (int)skill.GetValue(sheet);

            if (amount > 0)
            {
                if (skillAmount + amount <= 100)
                    skill.SetValue(sheet, skillAmount + amount);
                else if (skillAmount + amount > 100)
                    skill.SetValue(sheet, 100);
            }
            else
            {
                if (skillAmount - amount >= 1)
                    skill.SetValue(sheet, skillAmount - amount);
                else if (skillAmount - amount < 1)
                    skill.SetValue(sheet, 1);
            }
        }

        enum TagType
        {
            Violent,
            Handy,
            Diplomatic
        }

        private static readonly string[] ViolentSkills =
{
            Globals.SKILL_NAMES[1], // Energy Weapons
            Globals.SKILL_NAMES[2], // Explosives
            Globals.SKILL_NAMES[3], // Guns
            Globals.SKILL_NAMES[6], // Melee Weapons
            Globals.SKILL_NAMES[12], // Unarmed
        };

        private static readonly string[] HandySkills =
        {
            Globals.SKILL_NAMES[4], // Lockpick
            Globals.SKILL_NAMES[5], // Medicine
            Globals.SKILL_NAMES[7], // Repair
            Globals.SKILL_NAMES[11], // Survival
        };

        private static readonly string[] DiplomaticSkills =
        {
            Globals.SKILL_NAMES[0], // Barter
            Globals.SKILL_NAMES[8], // Science
            Globals.SKILL_NAMES[10], // Speech
            Globals.SKILL_NAMES[9], // Sneak
        };

        private static string GetRandomTag(TagType tagType)
        {
            switch (tagType)
            {
                case TagType.Violent:
                    return ViolentSkills[rand.Next(0, ViolentSkills.Length)];
                case TagType.Handy:
                    return HandySkills[rand.Next(0, HandySkills.Length)];
                case TagType.Diplomatic:
                    return DiplomaticSkills[rand.Next(0, DiplomaticSkills.Length)];
                default:
                    return null;
            }
        }

        /// <summary>
        /// Calculates a character's initial skills and tags three random skills from a predetermined set.
        /// </summary>
        /// <param name="character">The character to calculate initial skills for with random tags.</param>
        /// <param name="skill1">The type of the first random skill to tag.</param>
        /// <param name="skill2">The type of the second random skill to tag.</param>
        /// <param name="skill3">The type of the third random skill to tag.</param>
        /// <returns>A SkillSheet with values calculated with the character's SPECIAL and somewhat randomized tags.</returns>
        private static SkillSheet GetRandomInitialSkills(Character character, TagType skill1, TagType skill2, TagType skill3)
        {
            string[] tags = new string[3];

            tags[0] = GetRandomTag(skill1);
            tags[1] = GetRandomTag(skill2);
            tags[2] = GetRandomTag(skill3);

            while (tags[0].Equals(tags[1]))
                tags[1] = GetRandomTag(skill2);

            // tags[0] and tags[1] are definitely different by this point...
            // ...but tag[2] could equal tag[1] or tag[0]
            while (tags[2].Equals(tags[0]) || tags[2].Equals(tags[1]))
                tags[2] = GetRandomTag(skill3);

            return SkillsService.GetInitialSkills(character, tags[0], tags[1], tags[2]);
        }

        private static int RngAdd(int highest)
        {
            return rand.Next(0, highest + 1);
        }
    }
}
