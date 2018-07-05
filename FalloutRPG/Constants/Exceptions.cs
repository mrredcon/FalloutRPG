using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutRPG.Constants
{
    public class Exceptions
    {
        // Character Exception Message
        public const String CHAR_DISCORDID_EXISTS = "A character already exists with specified Discord ID.";
        public const String CHAR_NAMES_NOT_LETTERS = "Both character names must only consist of letters.";
        public const String CHAR_NAMES_LENGTH = "Both character names must be between 2-24 letters each.";
        public const String CHAR_SPECIAL_LENGTH = "The input special length did not equal 7.";
        public const String CHAR_SPECIAL_DOESNT_ADD_UP = "SPECIAL does not add up to 40.";
        public const String CHAR_SPECIAL_NOT_FOUND = "Unable to find SPECIAL for that character.";
        public const String CHAR_INVALID_TAG_NAMES = "One or more tag names were invalid.";
        public const String CHAR_INVALID_SKILL_NAME = "Skill name was invalid.";
        public const String CHAR_TAGS_NOT_UNIQUE = "One or more tag names were identical.";
        public const String CHAR_CHARACTER_IS_NULL = "Character is null.";
        public const String CHAR_NOT_ENOUGH_SKILL_POINTS = "Character does not have enough skill points.";
        public const String CHAR_SKILL_POINTS_GOES_OVER_MAX = "Unable to add skill points because it will take the skill above the max level.";
        public const String CHAR_SKILLS_NOT_SET = "Character skills aren't set.";
    }
}
