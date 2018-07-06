using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutRPG.Constants
{
    public class Pages
    {
        #region Character Help Pages
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
        #endregion

        #region Roll Help Pages
        public static string[] HELP_ROLL_PAGE1_TITLES = new string[]
        {
            "!roll [skill]",
            "!roll [special]"
        };

        public static string[] HELP_ROLL_PAGE1_CONTENTS = new string[]
        {
            "Gets a roll result based on the skill level.",
            "Gets a roll result based on the SPECIAL level."
        };
        #endregion
    }
}
