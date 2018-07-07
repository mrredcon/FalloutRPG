using FalloutRPG.Constants;
using FalloutRPG.Data.Repositories;
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
                throw new Exception(Exceptions.CHAR_DISCORDID_EXISTS);

            if (!StringTool.IsOnlyLetters(firstName) || !StringTool.IsOnlyLetters(lastName))
                throw new Exception(Exceptions.CHAR_NAMES_NOT_LETTERS);

            if (firstName.Length > 24 || lastName.Length > 24 || firstName.Length < 2 || lastName.Length < 2)
                throw new Exception(Exceptions.CHAR_NAMES_LENGTH);

            firstName = StringTool.ToTitleCase(firstName);
            lastName = StringTool.ToTitleCase(lastName);

            var character = new Character()
            {
                DiscordId = discordId,
                FirstName = firstName,
                LastName = lastName,
                Description = "",
                Story = "",
                Experience = 0,
                SkillPoints = 0,
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
            if (character == null)
                throw new ArgumentNullException("character");

            await _charRepository.SaveAsync(character);
        }
    }
}

