using FalloutRPG.Constants;
using FalloutRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FalloutRPG.Services.Roleplay
{
    public class NpcService
    {
        private readonly SkillsService _skillsService;
        private readonly RollService _rollService;

        private readonly List<Character> Npcs;
        
        public NpcService(SkillsService skillsService, RollService rollService)
        {
            _skillsService = skillsService;
            _rollService = rollService;

            Npcs = new List<Character>();
        }

        public void CreateNpc(string npcType, string firstName)
        {
            if (Npcs.Find(x => x.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase)) != null)
                throw new Exception(Exceptions.NPC_CHARACTER_EXISTS);

            var typeEnum = IsValidNpcType(npcType);

            Character character = new Character
            {
                FirstName = firstName,
                Special = GenerateNpcSpecial(typeEnum),
            };

            character.Skills = GenerateNpcSkills(typeEnum, character);

            Npcs.Add(character);
        }

        public string RollNpcSkill(string firstName, string skill)
        {
            var character = Npcs.Find(x => x.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase));

            if (character == null)
            {
                return Messages.FAILURE_EMOJI + "NPC name not found.";
            }
            else if (character.Skills == null)
            {
                throw new Exception(Exceptions.NPC_NULL_SKILLS);
            }

            return _rollService.GetSkillRollResult(skill, character) + " (\uD83D\uDCBBNPC)";
        }

        public string RollNpcSpecial(string firstName, string special)
        {
            var character = Npcs.Find(x => x.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase));

            if (character == null)
            {
                return Messages.FAILURE_EMOJI + "NPC was not found with given name.";
            }
            else if (character.Skills == null)
            {
                throw new Exception(Exceptions.NPC_NULL_SPECIAL);
            }

            return _rollService.GetSpecialRollResult(special, character) + " (\uD83D\uDCBBNPC)";
        }

        public NpcType IsValidNpcType(string typeString)
        {
            typeString = typeString.Trim();

            if (Enum.TryParse(value: typeString, ignoreCase: true, result: out NpcType typeEnum))
                return typeEnum;

            throw new Exception(Exceptions.NPC_INVALID_TYPE);
        }

        public enum NpcType
        {
            Error,
            Raider,
            RaiderVeteran,
            Mercenary,
            Spy,
            Merchant,
            Protectron,
            Assaultron,
            Eyebot,
            MisterHandy,
            MisterGutsy,
            Robobrain,
            SentryBot,
            Securitron,
            SuperMutant,
            FeralGhoul,
        }

        public Special GenerateNpcSpecial(NpcType npcType)
        {
            switch (npcType)
            {
                case NpcType.Raider:
                    return NpcSpecialPresets.Raider;
                case NpcType.RaiderVeteran:
                    return NpcSpecialPresets.RaiderVeteran;
                case NpcType.Mercenary:
                    return NpcSpecialPresets.Mercenary;
                case NpcType.Merchant:
                    return NpcSpecialPresets.Merchant;
                case NpcType.Spy:
                    return NpcSpecialPresets.Spy;
                case NpcType.SuperMutant:
                    return NpcSpecialPresets.SuperMutant
                case NpcType.FeralGhoul:
                    return NpcSpecialPresets.FeralGhoul
                case NpcType.Protectron:
                    return NpcSpecialPresets.Protectron
                case NpcType.Assaultron:
                    return NpcSpecialPresets.Assaultron
                case NpcType.Eyebot:
                    return NpcSpecialPresets.Eyebot
                case NpcType.MisterHandy:
                    return NpcSpecialPresets.MisterHandy
                case NpcType.MisterGutsy:
                    return NpcSpecialPresets.MisterGutsy
                case NpcType.Robobrain:
                    return NpcSpecialPresets.Robobrain
                case NpcType.SentryBot:
                    return NpcSpecialPresets.SentryBot
                case NpcType.Securitron:
                    return NpcSpecialPresets.Securitron
                default:
                    throw new ArgumentException("Given NpcType was invalid.", "npcType");
            }
        }

        public SkillSheet GenerateNpcSkills(NpcType npcType, Character character)
        {
            string[] tags = new string[3];

            SkillSheet skills = null;
            bool tagsAlreadySet = false;

            switch (npcType)
            {
                case NpcType.Raider:
                    {
                        tags = GetRandomTags(TagType.Violent, TagType.Violent, TagType.Violent);
                        break;
                    }
                case NpcType.RaiderVeteran:
                    {
                        tags = GetRandomTags(TagType.Violent, TagType.Violent, TagType.Handy);
                        break;
                    }
                case NpcType.Mercenary:
                    {
                        tags = GetRandomTags(TagType.Violent, TagType.Handy, TagType.Diplomatic);

                        skills = _skillsService.GetInitialSkills(character, tags[0], tags[1], tags[2]);
                        tagsAlreadySet = true;
                        skills.Barter += 15;
                        break;
                    }
                case NpcType.Merchant:
                case NpcType.Protectron:
                case NpcType.Assaultron:
                case NpcType.Eyebot:
                case NpcType.MisterHandy:
                case NpcType.MisterGutsy:
                case NpcType.Robobrain:
                case NpcType.SentryBot:
                case NpcType.Securitron:
                case NpcType.SuperMutant:
                case NpcType.FeralGhoul:
                default:
                    throw new ArgumentException(Exceptions.NPC_INVALID_TYPE);
            }

            if (!tagsAlreadySet)
                skills = _skillsService.GetInitialSkills(character, tags[0], tags[1], tags[2]);

            return skills;
        }

        private string[] GetRandomTags(TagType skill1, TagType skill2, TagType skill3)
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

            return tags;
        }

        private string GetRandomTag(TagType tagType)
        {
            Random rand = new Random();

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
    }
}
