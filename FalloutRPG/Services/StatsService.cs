using FalloutRPG.Constants;
using FalloutRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class StatsService
    {
        private const int DEFAULT_SPECIAL_POINTS = 40;

        private readonly CharacterService _charService;

        public StatsService(CharacterService charService)
        {
            _charService = charService;
        }

        public bool IsSpecialSet(Character character)
        {
            var properties = character.Special.GetType().GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.Equals("CharacterId") || prop.Name.Equals("Id"))
                    continue;

                var value = Convert.ToInt32(prop.GetValue(character.Special));
                if (value == 0) return false;
            }

            return true;
        }

        public async Task SetInitialSpecialAsync(Character character, int[] special)
        {
            if (special.Length != 7)
                throw new ArgumentException(Messages.EXC_SPECIAL_LENGTH);

            if (special.Sum() != DEFAULT_SPECIAL_POINTS)
                throw new ArgumentException(Messages.EXC_SPECIAL_DOESNT_ADD_UP);

            character.Special.Strength = special[0];
            character.Special.Perception = special[1];
            character.Special.Endurance = special[2];
            character.Special.Charisma = special[3];
            character.Special.Intelligence = special[4];
            character.Special.Agility = special[5];
            character.Special.Luck = special[6];

            await _charService.SaveCharacterAsync(character);
        }

        public bool AreSkillsSet(Character character)
        {
            var properties = character.Skills.GetType().GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.Equals("CharacterId") || prop.Name.Equals("Id"))
                    continue;

                var value = Convert.ToInt32(prop.GetValue(character.Skills));
                if (value == 0) return false;
            }

            return true;
        }

        public async Task SetTagSkills(Character character, string tag1, string tag2, string tag3)
        {
            if (!IsSpecialSet(character))
                throw new ArgumentException(Messages.EXC_SPECIAL_NOT_FOUND);

            if (!IsValidTagName(tag1) || !IsValidTagName(tag2) || !IsValidTagName(tag3))
                throw new ArgumentException(Messages.EXC_INVALID_TAG_NAMES);

            if (!AreUniqueTags(tag1, tag2, tag3))
                throw new ArgumentException(Messages.EXC_TAGS_NOT_UNIQUE);

            SetInitialSkills(character);

            SetTagSkill(character, tag1);
            SetTagSkill(character, tag2);
            SetTagSkill(character, tag3);

            await _charService.SaveCharacterAsync(character);
        }

        private bool IsValidTagName(string tag)
        {
            foreach (var name in Globals.SKILL_NAMES)
                if (tag.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;
        }

        private bool AreUniqueTags(string tag1, string tag2, string tag3)
        {
            if (tag1.Equals(tag2, StringComparison.InvariantCultureIgnoreCase) ||
                tag1.Equals(tag3, StringComparison.InvariantCultureIgnoreCase) ||
                tag2.Equals(tag3, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }

        private void SetTagSkill(Character character, string tag)
        {
            var properties = character.Skills.GetType().GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.Equals(tag, StringComparison.InvariantCultureIgnoreCase))
                {
                    prop.SetValue(character.Skills, (Convert.ToInt32(prop.GetValue(character.Skills)) + 15));
                }
            }
        }

        private int CalculateSkill(int stat, int luck)
        {
            return (2 + (2 * stat) + (luck / 2));
        }

        private void SetInitialSkills(Character character)
        {
            character.Skills.Barter = CalculateSkill(character.Special.Charisma, character.Special.Luck);
            character.Skills.EnergyWeapons = CalculateSkill(character.Special.Perception, character.Special.Luck);
            character.Skills.Explosives = CalculateSkill(character.Special.Perception, character.Special.Luck);
            character.Skills.Guns = CalculateSkill(character.Special.Agility, character.Special.Luck);
            character.Skills.Lockpick = CalculateSkill(character.Special.Perception, character.Special.Luck);
            character.Skills.Medicine = CalculateSkill(character.Special.Intelligence, character.Special.Luck);
            character.Skills.MeleeWeapons = CalculateSkill(character.Special.Strength, character.Special.Luck);
            character.Skills.Repair = CalculateSkill(character.Special.Intelligence, character.Special.Luck);
            character.Skills.Science = CalculateSkill(character.Special.Intelligence, character.Special.Luck);
            character.Skills.Sneak = CalculateSkill(character.Special.Agility, character.Special.Luck);
            character.Skills.Speech = CalculateSkill(character.Special.Charisma, character.Special.Luck);
            character.Skills.Survival = CalculateSkill(character.Special.Endurance, character.Special.Luck);
            character.Skills.Unarmed = CalculateSkill(character.Special.Endurance, character.Special.Luck);
        }
    }
}