namespace FalloutRPG.Constants
{
    public class Messages
    {
        public const string FAILURE_EMOJI = "\u274C";
        public const string SUCCESS_EMOJI = "\uD83D\uDC4D";
        public const string STAR_EMOJI = "\u2B50";

        // Character Messages
        public const string CHAR_CREATED_SUCCESS = SUCCESS_EMOJI + "Character created successfully. ({0})";
        public const string CHAR_STORY_SUCCESS = SUCCESS_EMOJI + "Character story updated successfully. ({0})";
        public const string CHAR_DESC_SUCCESS = SUCCESS_EMOJI + "Character description updated successfully. ({0})";

        // Stats Messages
        public const string EXP_LEVEL_UP = "Congratulations {0}, you have just advanced to level {1}!";
        public const string SKILLS_LEVEL_UP = "Hey, {0}! You have {1} unspent skill points. Spend them with *!char skills spend [skill] [points]*";
        public const string SKILLS_SPEND_POINTS_SUCCESS = SUCCESS_EMOJI + "Skill points added successfully. ({0})";
        public const string SKILLS_SET_SUCCESS = SUCCESS_EMOJI + "Character skills set successfully. ({0})";
        public const string SPECIAL_SET_SUCCESS = SUCCESS_EMOJI + "Character SPECIAL set successfully. ({0})";

        // Character Error Messages
        public const string ERR_CHAR_NOT_FOUND = FAILURE_EMOJI + "Unable to find character. ({0})";
        public const string ERR_STORY_NOT_FOUND = FAILURE_EMOJI + "Unable to find character story. ({0})";
        public const string ERR_DESC_NOT_FOUND = FAILURE_EMOJI + "Unable to find character description. ({0})";
        public const string ERR_SPECIAL_NOT_FOUND = FAILURE_EMOJI + "Unable to find character SPECIAL. ({0})";

        // Stats Error Messages
        public const string ERR_SKILLS_NOT_FOUND = FAILURE_EMOJI + "Unable to find character skills. ({0})";
        public const string ERR_SKILLS_ALREADY_SET = FAILURE_EMOJI + "Character skills are already set. ({0})";
        public const string ERR_SPECIAL_ALREADY_SET = FAILURE_EMOJI + "Character SPECIAL is already set. ({0})";
    }
}
