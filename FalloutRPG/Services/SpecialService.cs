using FalloutRPG.Constants;
using FalloutRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class SpecialService
    {
        private const int DEFAULT_SPECIAL_POINTS = 40;

        private readonly CharacterService _charService;

        public SpecialService(CharacterService charService)
        {
            _charService = charService;
        }

        /// <summary>
        /// Set character's special.
        /// </summary>
        public async Task SetInitialSpecialAsync(Character character, int[] special)
        {
            if (character == null)
                throw new ArgumentNullException(Exceptions.CHAR_CHARACTER_IS_NULL);

            if (special.Length != 7)
                throw new ArgumentException(Exceptions.CHAR_SPECIAL_LENGTH);

            if (special.Sum() != DEFAULT_SPECIAL_POINTS)
                throw new ArgumentException(Exceptions.CHAR_SPECIAL_DOESNT_ADD_UP);

            character.Special.Strength = special[0];
            character.Special.Perception = special[1];
            character.Special.Endurance = special[2];
            character.Special.Charisma = special[3];
            character.Special.Intelligence = special[4];
            character.Special.Agility = special[5];
            character.Special.Luck = special[6];

            await _charService.SaveCharacterAsync(character);
        }

        /// <summary>
        /// Checks if a character's special has been set.
        /// </summary>
        public bool IsSpecialSet(Character character)
        {
            if (character == null)
                throw new ArgumentNullException(Exceptions.CHAR_CHARACTER_IS_NULL);

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
    }
}
