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
                Special = (Special)typeof(NpcSpecialPresets).GetProperty(typeEnum.ToString()).GetValue(null),
            };

            character.Skills = (SkillSheet)typeof(NpcSkillPresets).GetMethod(typeEnum.ToString()).Invoke(null, new object[]{ character });

            if (character.Skills == null)
                throw new Exception(Exceptions.NPC_INVALID_TYPE);

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

            string result = _rollService.GetSkillRollResult(skill, character);

            // Check for null, since GetSkillRollResult will return null if any Skills are 0
            if (result != null)
                return result + " (\uD83D\uDCBBNPC)";
            else
                return $"{Messages.FAILURE_EMOJI} {firstName} can't use this Skill!";
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
    }
}
