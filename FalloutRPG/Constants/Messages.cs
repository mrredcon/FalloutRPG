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

        // Gambling Messages
        public const string BET_PLACED = SUCCESS_EMOJI + "{0}, bet placed!";

        // Gambling Error Messages
        public const string ERR_BALANCE_ADD_FAIL = FAILURE_EMOJI + "{0}, I failed to add your balance to the directory! (Do you have a character?)";
        public const string ERR_BET_TOO_HIGH = FAILURE_EMOJI + "{0}, you don't have enough money for that bet!";
        public const string ERR_BET_TOO_LOW = FAILURE_EMOJI + "{0}, you have to bet something!";

        // Craps Messages
        public const string CRAPS_CRAPOUT = "{0} crapped out!";
        public const string CRAPS_CRAPOUT_POS = "{0} crapped out, but they were counting on it!";
        public const string CRAPS_CRAPOUT_PUSH = "{0} crapped out, but the House always gets the best odds!";
        public const string CRAPS_NATURAL = "{0} rolled a Natural!";
        public const string CRAPS_NATURAL_NEG = "{0} rolled a Natural but they wish they hadn't!";
        public const string CRAPS_POINT_ROUND = "{0} advanced the round into the Point!";
        public const string CRAPS_POINT_SET = "{0}'s point is {1}";
        public const string CRAPS_POINT_ROLL = "{0} rolled their point!";
        public const string CRAPS_POINT_ROLL_NEG = "{0} rolled their point, but they betted against it!";
        public const string CRAPS_SEVEN_OUT = "{0} sevened out!";
        public const string CRAPS_SEVEN_OUT_POS = "{0} sevened out, but they were counting on it!";
        public const string CRAPS_NEW_SHOOTER = "{0} is the new shooter.";
        public const string CRAPS_JOIN_MATCH = "{0} joins the match!";
        public const string CRAPS_LEAVE_MATCH = "{0} left the match.";

        // Craps Error Messages
        public const string ERR_CRAPS_NOT_SHOOTER = FAILURE_EMOJI + "{0}, you are not the shooter! (Join the match and wait.)";
        public const string ERR_CRAPS_BET_NOT_SET = FAILURE_EMOJI + "{0}, you do not have a bet placed!";
        public const string ERR_CRAPS_BET_ALREADY_SET = FAILURE_EMOJI + "{0}, you already have a bet!";
        public const string ERR_CRAPS_BET_PARSE_FAIL = FAILURE_EMOJI + "{0}, valid bet types are: 'pass', 'dontpass', 'come', or 'dontcome' (without single quotes.)";
        public const string ERR_CRAPS_POINT_NOT_SET = FAILURE_EMOJI + "{0}, you can't place a Come bet when the Point isn't set!";
        public const string ERR_CRAPS_POINT_ALREADY_SET = FAILURE_EMOJI + "{0}, you can't place a Pass bet after the Point has been set!";
        public const string ERR_CRAPS_JOIN_FAIL = FAILURE_EMOJI + "Failed to join {0} into the match.";
        public const string ERR_CRAPS_LEAVE_FAIL = FAILURE_EMOJI + "Failed to remove {0} match, are they the shooter?";
    }
}
