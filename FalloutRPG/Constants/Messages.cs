using System;

namespace FalloutRPG.Constants
{
    public class Messages
    {
        // Character Messages
        public const String CHAR_CREATED_SUCCESS = "\uD83D\uDC4D Character created successfully. ({0})";
        public const String CHAR_STORY_SUCCESS = "\uD83D\uDC4D Character story updated successfully. ({0})";
        public const String CHAR_DESC_SUCCESS = "\uD83D\uDC4D Character description updated successfully. ({0})";
        public const String CHAR_SPEND_POINTS_SUCCESS = "\uD83D\uDC4D Skill points added successfully. ({0})";

        // Experience Messages
        public const String EXP_LEVEL_UP = "\u2B50 Congratulations {0}, you have just advanced to level {1}!";

        // Character Error Messages
        public const String ERR_CHAR_NOT_FOUND = "\u274C Unable to find character. ({0})";
        public const String ERR_STORY_NOT_FOUND = "\u274C Unable to find character story. ({0})";
        public const String ERR_DESC_NOT_FOUND = "\u274C Unable to find character description. ({0})";
        public const String ERR_SPECIAL_NOT_FOUND = "\u274C Unable to find character SPECIAL. ({0})";
        public const String ERR_SKILLS_NOT_FOUND = "\u274C Unable to find character skills. ({0})";

        // Fallout Character Message
        public const String CHAR_SPECIAL_SUCCESS = "\uD83D\uDC4D S.P.E.C.I.A.L. set successfully. ({0})";
        public const String CHAR_SKILLS_SETSUCCESS = "\uD83D\uDC4D Skills set and tagged successfully! ({0})";

        // Fallout Character Error Message
        public const String ERR_SPECIAL_EXISTS = "\u274C You already have a S.P.E.C.I.A.L. set. ({0})";
        public const String ERR_SPECIAL_INVALID = "\u274C S.P.E.C.I.A.L. was invalid. (Is it in range or set?) ({0})";
        public const String ERR_SPECIAL_PARSE = "\u274C Could not parse given S.P.E.C.I.A.L. input. ({0})";
        public const String ERR_SKILLS_ALREADYSET = "\u274C Your skills are already set! ({0})";
        public const String ERR_SKILLS_NOTSET = "\u274C Your skills are not set! ({0})";

        // Fallout Character Exception Message
        public const String EXC_SKILLS_TAGSNOTUNIQUE = "\u274C All tagged skills must be unique. ({0})";
        public const String EXC_SKILLS_TAGSINVALID = "\u274C Could not recongnize one or more given skills. ({0})";

        // Help Pages
        public static string[] HELP_CHAR_PAGE1_TITLES = new string[]
        {
            "!char show",
            "!char show [@user]",
            "!char create [forename] [surname]",
            "!char story",
            "!char story [@user]",
            "!char story update [story]",
            "!char desc",
            "!char desc [@user]",
            "!char desc update [desc]"
        };

        public static string[] HELP_CHAR_PAGE1_CONTENTS = new string[]
        {
            "Displays your character.",
            "Displays specified user's character.",
            "Creates your character.",
            "Displays your character's story.",
            "Displays specified user's character story.",
            "Updates your character's story.",
            "Displays your character's description.",
            "Displays specified user's character description.",
            "Updates your character's description."
        };

        public static string[] HELP_CHAR_PAGE2_TITLES = new string[]
        {
            "!char stats",
            "!char stats [@user]",
            "!char highscores",
            "!char skills",
            "!char skills [@user]",
            "!char skills set [tag1] [tag2] [tag3]",
            "!char skills spend [skill] [points]",
            "!char special",
            "!char special [@user]",
            "!char special set [S] [P] [E] [C] [I] [A] [L]"
        };

        public static string[] HELP_CHAR_PAGE2_CONTENTS = new string[]
        {
            "Displays your level and experience.",
            "Displays specified user's level and experience.",
            "Displays the top 10 players ordered by experience.",
            "Displays your character's skills.",
            "Displays specified user's character skills.",
            "Sets your initial tag skills.",
            "Puts points in one of your skills.",
            "Displays your character's SPECIAL.",
            "Displays specified user's character SPECIAL.",
            "Sets your SPECIAL."
        };
}
}
