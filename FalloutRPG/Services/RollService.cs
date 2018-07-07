using Discord;
using FalloutRPG.Constants;
using FalloutRPG.Models;
using System;
using System.Text;

namespace FalloutRPG.Services
{
    public class RollService
    {
        private readonly CharacterService _charService;
        private readonly SpecialService _specService;
        private readonly SkillsService _skillsService;

        public RollService(CharacterService charService, SpecialService specService, SkillsService skillsService)
        {
            _charService = charService;
            _specService = specService;
            _skillsService = skillsService;
        }

        public string GetSpRoll(IUser user, string specialToRoll)
        {
            var character = _charService.GetCharacter(user.Id);

            if (character == null)
            {
                return String.Format(Messages.ERR_CHAR_NOT_FOUND, user.Mention);
            }

            if (!_specService.IsSpecialSet(character))
            {
                return String.Format(Messages.ERR_SPECIAL_NOT_FOUND, user.Mention);
            }

            return GetSpecialRollResult(specialToRoll, character);
        }

        public string GetSkillRoll(IUser user, String skillToRoll)
        {
            var character = _charService.GetCharacter(user.Id);

            if (character == null)
            {
                return String.Format(Messages.ERR_CHAR_NOT_FOUND, user.Mention);
            }

            if (!_specService.IsSpecialSet(character))
            {
                return String.Format(Messages.ERR_SPECIAL_NOT_FOUND, user.Mention);
            }

            if (!_skillsService.AreSkillsSet(character))
            {
                return String.Format(Messages.ERR_SKILLS_NOT_FOUND, user.Mention);
            }

            return GetSkillRollResult(skillToRoll, character);
        }

        public string GetSkillRollResult(String skill, Character character)
        {
            Random rand = new Random();

            var charSkills = character.Skills;
            var special = character.Special;

            if (charSkills.Barter == 0) return null;

            var skillProp = typeof(SkillSheet).GetProperty(skill);
            int skillValue = (int)skillProp.GetValue(charSkills);

            // RNG influenced by character luck except when its 5
            int rngResult = (int)Math.Round((rand.Next(1, 101) * (1.0 - (special.Luck / 10.0 - .5))));

            // compares your roll with your skills, and how much better you did than the bare minimum
            double resultPercent = (double)(skillValue - rngResult) / skillValue;
            resultPercent = Math.Round(resultPercent, 2) * 100;

            Console.WriteLine("RNG: " + rngResult + " SKILL: " + skillValue + " SP: " + resultPercent);

            if (rngResult <= resultPercent || rngResult <= skillValue)
                return GetRollMessage(character.FirstName, skill, true, (int)resultPercent);
            else
                return GetRollMessage(character.FirstName, skill, false, (int)resultPercent * -1);
        }

        public string GetSpecialRollResult(String rollSpecial, Character character)
        {
            Random rand = new Random();
            //var skills = CharacterUtilityService.GetCharacterSkills(user);
            var charSpecial = character.Special;
            if (charSpecial.Strength == 0) return null;

            // RNG influenced by character luck except when its 5
            int rngResult = (int)Math.Round((rand.Next(1, 11) * (1.0 - (charSpecial.Luck / 10.0 - .5)))),
                specialValue = (int)typeof(Special).GetProperty(rollSpecial).GetValue(charSpecial);

            int difference = specialValue - rngResult;
            // compares your roll with your skills, and how much better you did than the bare minimum
            double successPercent = (double)difference / specialValue;
            successPercent = Math.Round(successPercent, 2) * 100;

            Console.WriteLine("RNG: " + rngResult + " SPECIAL: " + specialValue + " SP: " + successPercent);

            // TODO: maybe tell user what they rolled? (needed specialValue, rolled rngResult)
            if (rngResult <= successPercent || rngResult <= specialValue)
                return GetRollMessage(character.FirstName, rollSpecial, true, (int)successPercent);
            else
                return GetRollMessage(character.FirstName, rollSpecial, false, (int)successPercent * -1);
        }

        private string GetRollMessage(string charName, string roll, bool success, int percent)
        {
            var result = new StringBuilder();

            if (success)
            {
                if (percent >= 90)
                {
                    // criticaler success (holy shit)
                    result.Append($"**CRITICAL {roll.ToUpper()} SUCCESS!!!**");
                }
                else if (percent >= 80)
                {
                    // critical success
                    result.Append($"**CRITICAL {roll.ToUpper()} SUCCESS!**");
                }
                else if (percent >= 60)
                {
                    // purty good (great) success
                    result.Append($"__GREAT {roll.ToUpper()} SUCCESS__");
                }
                else if (percent >= 40)
                {
                    // good success
                    result.Append($"*Very good {roll} success*");
                }
                else if (percent >= 25)
                {
                    // decent
                    result.Append($"*Good {roll} success*");
                }
                else if (percent >= 10)
                {
                    // decent
                    result.Append($"*Above average {roll} success*");
                }
                else
                {
                    // close call!
                    result.Append($"__***CLOSE CALL! {roll} success***__");
                }

                result.Append($" for {charName}: did **{percent}%** better than needed!");
            }
            else
            {
                if (percent >= 90)
                {
                    // criticaler failure (holy shit
                    result.Append($"**CRITICAL {roll.ToUpper()} FAILURE!!!**");
                }
                else if (percent >= 80)
                {
                    // critical failure
                    result.Append($"**CRITICAL {roll.ToUpper()} FAILURE!**");
                }
                else if (percent >= 60)
                {
                    // purty good (great) failure
                    result.Append($"__TERRIBLE {roll.ToUpper()} FAILURE__");
                }
                else if (percent >= 40)
                {
                    // good failure
                    result.Append($"*Pretty bad {roll} failure*");
                }
                else if (percent >= 25)
                {
                    // decent
                    result.Append($"*Bad {roll} failure*");
                }
                else if (percent >= 10)
                {
                    // decent
                    result.Append($"*Above average {roll} failure*");
                }
                else
                {
                    // close call!
                    result.Append($"__***Heartbreaking {roll} failure***__");
                }

                result.Append($" for {charName}: did **{percent}%** worse than needed!");
            }

            return result.ToString();
        }
    }
}
