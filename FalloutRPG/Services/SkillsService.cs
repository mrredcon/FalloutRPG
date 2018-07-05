using FalloutRPG.Constants;
using FalloutRPG.Models;
using System;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class SkillsService
    {
        private const int DEFAULT_SKILL_POINTS = 10;
        private const int MAX_SKILL_LEVEL = 100;

        private readonly CharacterService _charService;
        private readonly SpecialService _specService;

        public SkillsService(CharacterService charService, SpecialService specService)
        {
            _charService = charService;
            _specService = specService;
        }

        /// <summary>
        /// Set character's tag skills.
        /// </summary>
        public async Task SetTagSkills(Character character, string tag1, string tag2, string tag3)
        {
            if (character == null)
                throw new ArgumentNullException(Exceptions.CHAR_CHARACTER_IS_NULL);

            if (!_specService.IsSpecialSet(character))
                throw new ArgumentException(Exceptions.CHAR_SPECIAL_NOT_FOUND);

            if (!IsValidSkillName(tag1) || !IsValidSkillName(tag2) || !IsValidSkillName(tag3))
                throw new ArgumentException(Exceptions.CHAR_INVALID_TAG_NAMES);

            if (!AreUniqueTags(tag1, tag2, tag3))
                throw new ArgumentException(Exceptions.CHAR_TAGS_NOT_UNIQUE);

            InitializeSkills(character);

            SetTagSkill(character, tag1);
            SetTagSkill(character, tag2);
            SetTagSkill(character, tag3);

            await _charService.SaveCharacterAsync(character);
        }

        /// <summary>
        /// Checks if character's skills are set.
        /// </summary>
        public bool AreSkillsSet(Character character)
        {
            if (character == null)
                throw new ArgumentNullException(Exceptions.CHAR_CHARACTER_IS_NULL);

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

        /// <summary>
        /// Gives character their skill points from leveling up.
        /// </summary>
        /// <remarks>
        /// Uses the Fallout New Vegas formula. (10 + (INT / 2))
        /// </remarks>
        public void GrantSkillPoints(Character character)
        {
            if (character == null)
                throw new ArgumentNullException(Exceptions.CHAR_CHARACTER_IS_NULL);

            var points = DEFAULT_SKILL_POINTS + (character.Special.Intelligence / 2);

            character.SkillPoints += points;
        }

        /// <summary>
        /// Puts an amount of points in a specified skill.
        /// </summary>
        public void PutPointsInSkill(Character character, string skill, int points)
        {
            if (character == null)
                throw new ArgumentNullException(Exceptions.CHAR_CHARACTER_IS_NULL);

            if (!AreSkillsSet(character))
                throw new Exception(Exceptions.CHAR_SKILLS_NOT_SET);

            if (!IsValidSkillName(skill))
                throw new ArgumentException(Exceptions.CHAR_INVALID_SKILL_NAME);

            if (points == 0) return;

            if (points > character.SkillPoints)
                throw new Exception(Exceptions.CHAR_NOT_ENOUGH_SKILL_POINTS);

            var properties = character.Skills.GetType().GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.Equals(skill, StringComparison.InvariantCultureIgnoreCase))
                {
                    var propSkill = Convert.ToInt32(prop.GetValue(character.Skills));

                    if ((propSkill + points) > MAX_SKILL_LEVEL)
                        throw new Exception(Exceptions.CHAR_SKILL_POINTS_GOES_OVER_MAX);

                    prop.SetValue(character.Skills, (propSkill + points));
                    character.SkillPoints -= points;
                    break;
                }
            }
        }

        /// <summary>
        /// Checks if the tag name matches any of the skill names.
        /// </summary>
        private bool IsValidSkillName(string skill)
        {
            foreach (var name in Globals.SKILL_NAMES)
                if (skill.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;
        }

        /// <summary>
        /// Checks if all the tags are unique.
        /// </summary>
        private bool AreUniqueTags(string tag1, string tag2, string tag3)
        {
            if (tag1.Equals(tag2, StringComparison.InvariantCultureIgnoreCase) ||
                tag1.Equals(tag3, StringComparison.InvariantCultureIgnoreCase) ||
                tag2.Equals(tag3, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }

        /// <summary>
        /// Sets a character's tag skill.
        /// </summary>
        private void SetTagSkill(Character character, string tag)
        {
            if (character == null)
                throw new ArgumentNullException(Exceptions.CHAR_CHARACTER_IS_NULL);

            var properties = character.Skills.GetType().GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.Equals(tag, StringComparison.InvariantCultureIgnoreCase))
                {
                    prop.SetValue(character.Skills, (Convert.ToInt32(prop.GetValue(character.Skills)) + 15));
                }
            }
        }

        /// <summary>
        /// Initializes a character's skills.
        /// </summary>
        private void InitializeSkills(Character character)
        {
            if (character == null)
                throw new ArgumentNullException(Exceptions.CHAR_CHARACTER_IS_NULL);

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

        /// <summary>
        /// Calculates a skill based on New Vegas formula.
        /// </summary>
        private int CalculateSkill(int stat, int luck)
        {
            return (2 + (2 * stat) + (luck / 2));
        }
    }
}
