using FalloutRPG.Constants;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Exceptions;
using FalloutRPG.Models;
using FalloutRPG.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class CharacterService
    {
        private string[] validNames = new string[]
        {
            "Barter",
            "EnergyWeapons",
            "Explosives",
            "Guns",
            "Lockpick",
            "Medicine",
            "MeleeWeapons",
            "Repair",
            "Science",
            "Sneak",
            "Speech",
            "Survival",
            "Unarmed"
        };

        private readonly IRepository<Character> _charRepository;
        private readonly IRepository<SkillSheet> _skillRepository;
        private readonly IRepository<Special> _specialRepository;

        public CharacterService(
            IRepository<Character> charRepository,
            IRepository<SkillSheet> skillRepository,
            IRepository<Special> specialRepository)
        {
            _charRepository = charRepository;
            _skillRepository = skillRepository;
            _specialRepository = specialRepository;
        }

        /// <summary>
        /// Gets a character from the repository by Discord ID.
        /// </summary>
        public Character GetCharacter(ulong discordId)
        {
            var character = _charRepository.Query.Where(x => x.DiscordId == discordId).FirstOrDefault();

            if (character == null) return null;

            character.Special = _specialRepository.Query.Where(x => x.CharacterId == character.Id).FirstOrDefault();
            character.Skills = _skillRepository.Query.Where(x => x.CharacterId == character.Id).FirstOrDefault();

            return character;
        }

        /// <summary>
        /// Creates a new character.
        /// </summary>
        public async Task<Character> CreateCharacterAsync(ulong discordId, string firstName, string lastName)
        {
            if (GetCharacter(discordId) != null)
                throw new CharacterException(Messages.EXC_DISCORDID_EXISTS);

            if (!StringTool.IsOnlyLetters(firstName) || !StringTool.IsOnlyLetters(lastName))
                throw new CharacterException(Messages.EXC_NAMES_NOT_LETTERS);

            if (firstName.Length > 24 || lastName.Length > 24 || firstName.Length < 2 || lastName.Length < 2)
                throw new CharacterException(Messages.EXC_NAMES_LENGTH);

            var character = new Character()
            {
                DiscordId = discordId,
                FirstName = firstName,
                LastName = lastName,
                Description = "",
                Story = "",
                Experience = 0,
                Special = new Special()
                {
                    Strength = 0,
                    Perception = 0,
                    Endurance = 0,
                    Charisma = 0,
                    Intelligence = 0,
                    Agility = 0,
                    Luck = 0
                },
                Skills = new SkillSheet()
                {
                    Barter = 0,
                    EnergyWeapons = 0,
                    Explosives = 0,
                    Guns = 0,
                    Lockpick = 0,
                    Medicine = 0,
                    MeleeWeapons = 0,
                    Repair = 0,
                    Science = 0,
                    Sneak = 0,
                    Speech = 0,
                    Survival = 0,
                    Unarmed = 0
                }
            };

            await _charRepository.AddAsync(character);

            return character;
        }

        /// <summary>
        /// Gets the top 10 characters with the most experience.
        /// </summary>
        public async Task<List<Character>> GetHighScoresAsync()
        {
            var characters = await _charRepository.FetchAllAsync();
            return characters.OrderByDescending(x => x.Experience).Take(10).ToList();
        }

        /// <summary>
        /// Saves a character.
        /// </summary>
        public async Task SaveCharacterAsync(Character character)
        {
            await _charRepository.SaveAsync(character);
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
                throw new ArgumentException("Special is not correct. Panic please.");

            if (special.Sum() != 40)
                throw new ArgumentException("Special does not add up to 40.");

            character.Special.Strength = special[0];
            character.Special.Perception = special[1];
            character.Special.Endurance = special[2];
            character.Special.Charisma = special[3];
            character.Special.Intelligence = special[4];
            character.Special.Agility = special[5];
            character.Special.Luck = special[6];

            await SaveCharacterAsync(character);
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
                throw new ArgumentException("Character SPECIAL not set.");

            if (!IsValidTagName(tag1) || !IsValidTagName(tag2) || !IsValidTagName(tag3))
                throw new ArgumentException("One or more invalid tag names.");

            if (!AreUniqueTags(tag1, tag2, tag3))
                throw new ArgumentException("Tags are not unique.");

            SetInitialSkills(character);

            SetTagSkill(character, tag1);
            SetTagSkill(character, tag2);
            SetTagSkill(character, tag3);

            await SaveCharacterAsync(character);
        }

        private bool IsValidTagName(string tag)
        {
            foreach (var name in validNames)
            {
                if (tag.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

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

