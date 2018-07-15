using FalloutRPG.Constants;
using FalloutRPG.Models;
using System;
using System.Collections.Generic;
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
                return "NPC didn't have skills for some reason bruh";
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
                return "NPC didn't have skills for some reason bruh";
            }

            return _rollService.GetSpecialRollResult(special, character) + " (\uD83D\uDCBBNPC)";
        }

        public Special GenerateNpcSpecial(NpcType npcType)
        {
            Random rand = new Random(); // Random upper bound is exclusive!

            switch (npcType)
            {
                case NpcType.Raider:
                    return new Special
                    {
                        // lowest special total = 29, highest special total = 44

                        Strength = 5 + rand.Next(0,3), // 5-7
                        Perception = 4 + rand.Next(0,3), // 4-6
                        Endurance = 5 + rand.Next(0,3), // 5-7
                        Charisma = 3 + rand.Next(0,3), // 3-5
                        Intelligence = 3 + rand.Next(0,4), // 3-6
                        Agility = 5 + rand.Next(0,3), // 5-7
                        Luck = 4 + rand.Next(0,3) // 4-6
                    };
                case NpcType.RaiderVeteran:
                    return new Special
                    {
                        // lowest SPECIAL total = 36, highest SPECIAL total = 47

                        Strength = 6 + rand.Next(0,3), // 6-8
                        Perception = 4 + rand.Next(0,3), // 4-6
                        Endurance = 6 + rand.Next(0,2), // 6-7
                        Charisma = 4 + rand.Next(0,2), // 4-5
                        Intelligence = 5 + rand.Next(0,3), // 5-7
                        Agility = 7 + rand.Next(0,2), // 7-8
                        Luck = 4 + rand.Next(0,3) // 4-6
                    };
                //case NpcType.RaiderSurvivalist:
                //    return new Special
                //    {
                //        Strength = rand.Next(6, 8),
                //        Perception = rand.Next(5, 7),
                //        Endurance = rand.Next(8, 11),
                //        Charisma = rand.Next(3, 6),
                //        Intelligence = rand.Next(6, 9),
                //        Agility = rand.Next(6, 8),
                //        Luck = rand.Next(6, 9)
                //    };
                case NpcType.Mercenary:
                    return new Special
                    {
                        // lowest SPECIAL total = 40, highest SPECIAL = 51

                        Strength = 5 + rand.Next(0,3), // 5-7
                        Perception = 6 + rand.Next(0,2), // 6-7
                        Endurance = 6 + rand.Next(0,3), // 6-8
                        Charisma = 6 + rand.Next(0,2), // 6-7
                        Intelligence = 6 + rand.Next(0,3), // 6-8
                        Agility = 7 + rand.Next(0,2), // 7-8
                        Luck = 4 + rand.Next(0,3) // 4-6
                    };
                default:
                    throw new ArgumentException("Given NpcType was invalid.", "npcType");
            }
        }
        public SkillSheet GenerateNpcSkills(NpcType npcType, Character character)
        {
            switch (npcType)
            {
                case NpcType.Raider:
                case NpcType.RaiderVeteran:
                    return _skillsService.GetInitialSkills(character, "explosives", "guns", "meleeweapons");
                case NpcType.Mercenary:
                    return _skillsService.GetInitialSkills(character, "guns", "speech", "energyweapons");
                default:
                    throw new ArgumentException(Exceptions.NPC_INVALID_TYPE);
            }
        }

        public enum NpcType
        {
            Error,
            Raider,
            RaiderVeteran,
            RaiderSurvivalist,
            Mercenary,
        }

        public NpcType IsValidNpcType(string typeString)
        {
            typeString = typeString.Trim();

            if (Enum.TryParse(value: typeString, ignoreCase: true, result: out NpcType typeEnum))
                return typeEnum;

            throw new Exception(Exceptions.NPC_INVALID_TYPE);
        }
    }
}
