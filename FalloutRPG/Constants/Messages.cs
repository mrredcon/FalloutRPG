using System;

namespace FalloutRPG.Constants
{
    public class Messages
    {
        // Character Messages
        public const String CHAR_CREATED_SUCCESS = "\uD83D\uDC4D Character created successfully. ({0})";
        public const String CHAR_STORY_SUCCESS = "\uD83D\uDC4D Character story updated successfully. ({0})";
        public const String CHAR_DESC_SUCCESS = "\uD83D\uDC4D Character description updated successfully. ({0})";

        // Experience Messages
        public const String EXP_LEVEL_UP = "\u2B50 Congratulations {0}, you have just advanced to level {1}!";

        // Character Error Messages
        public const String ERR_CHAR_NOT_FOUND = "\u274C Unable to find character. ({0})";
        public const String ERR_STORY_NOT_FOUND = "\u274C Unable to find character story. ({0})";
        public const String ERR_DESC_NOT_FOUND = "\u274C Unable to find character description. ({0})";

        // Character Exception Message
        public const String EXC_DISCORDID_EXISTS = "\u274C You already have a character. ({0})";
        public const String EXC_NAMES_NOT_LETTERS = "\u274C Both names must only consist of letters. ({0})";
        public const String EXC_NAMES_LENGTH = "\u274C Both names must be between 2-24 letters each. ({0})";

        // Fallout Character Message
        public const String CHAR_SPECIAL_SUCCESS = "\uD83D\uDC4D S.P.E.C.I.A.L. set successfully. ({0})";

        // Fallout Character Error Message
        public const String ERR_SPECIAL_EXISTS = "\u274C You already have a S.P.E.C.I.A.L. set. ({0})";
        public const String ERR_SPECIAL_INVALID = "\u274C Did not receive a valid S.P.E.C.I.A.L.. ({0})";
        public const String ERR_SPECIAL_PARSE = "\u274C Could not parse given S.P.E.C.I.A.L. input. ({0})";
    }
}
