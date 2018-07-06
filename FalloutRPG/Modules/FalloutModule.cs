using Discord;
using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Models;
using FalloutRPG.Services;
using System;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    public class Fallout76Module : ModuleBase<SocketCommandContext>
    {
        [Command("daysleft"), Ratelimit(1, 0.1, Measure.Minutes)]
        [Alias("countdown", "days")]
        public async Task DaysLeftAsync()
        {
            DateTime today = DateTime.Now;
            DateTime release = new DateTime(2018, 11, 14);
            TimeSpan span = (release - today);

            await Context.Channel.SendMessageAsync(
                $"There are {span.Days} days," +
                $" {span.Hours} hours," +
                $" {span.Minutes} minutes," +
                $" {span.Seconds} seconds " +
                $"and {span.Milliseconds} milliseconds left until release! (UTC)");
        }

        [RequireOwner]
        [Command("echo")]
        public async Task EchoAsync(string input)
        {
            await Context.Channel.SendMessageAsync(input);
        }
    }

    [Group("character")]
    [Alias("char")]
    public class FalloutCharacterModule : ModuleBase<SocketCommandContext>
    {
        [Group("special")]
        [Alias("sp")]
        public class FalloutCharacterSpecialModule : ModuleBase<SocketCommandContext>
        {
            private readonly CharacterService _charService;
            private readonly FalloutService _falloutService;

            public FalloutCharacterSpecialModule(
                CharacterService charService,
                FalloutService falloutService)
            {
                _charService = charService;
                _falloutService = falloutService;
            }

            [Command("view")]
            [Alias("view")]
            public async Task ViewSpecialAsync()
            {
                var c = _charService.GetCharacter(Context.User.Id);

                StringBuilder result = new StringBuilder();

                foreach (var prop in typeof(Special).GetProperties())
                {
                    if (prop.Name.Equals("CharacterId") || prop.Name.Equals("Id"))
                        continue;
                    result.Append(prop.Name + ": " + prop.GetValue(c.Special) + "\n");
                }

                var embed = Util.EmbedTool.BuildBasicEmbed("S.P.E.C.I.A.L. stats for " + Context.User.Username, result.ToString());

                await ReplyAsync(embed: embed);
            }
            [Command("set")]
            [Alias("set")]
            [Summary("Set a new character's S.P.E.C.I.A.L.")]
            public async Task SetCharacterSpecial(string newSpecial)
            {
                var userInfo = Context.User;
                var character = _charService.GetCharacter(userInfo.Id);

                if (character == null)
                {
                    await Context.Channel.SendMessageAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }
                // default should be all 0s which is is not a valid special, so a valid one means a special has been set.
                if (_falloutService.IsValidSpecial(character.Special))
                {
                    await ReplyAsync(String.Format(Messages.ERR_SPECIAL_EXISTS, userInfo.Mention));
                    return;
                }

                Special special = _falloutService.ParseSpecialString(newSpecial);

                // parsed properly
                if (special != null)
                {
                    // Success condition
                    if (_falloutService.IsValidSpecial(special, newChar: true))
                    {
                        character.Special = special;
                        await _charService.SaveCharacterAsync(character);
                        await ReplyAsync(String.Format(Messages.CHAR_SPECIAL_SUCCESS, userInfo.Mention));
                    }
                    else
                        // parsed special was not in range
                        await ReplyAsync(String.Format(Messages.ERR_SPECIAL_INVALID, userInfo.Mention));
                }
                else
                    // did not parse properly
                    await ReplyAsync(String.Format(Messages.ERR_SPECIAL_PARSE, userInfo.Mention));
            }
        }
        [Group("skills")]
        [Alias("sk")]
        public class FalloutCharacterSkillsModule : ModuleBase<SocketCommandContext>
        {
            private readonly CharacterService _charService;
            private readonly FalloutService _falloutService;

            public FalloutCharacterSkillsModule(
                CharacterService charService,
                FalloutService falloutService)
            {
                _charService = charService;
                _falloutService = falloutService;
            }

            [Command("view")]
            [Alias("view")]
            public async Task ViewSkillsAsync()
            {
                var c = _charService.GetCharacter(Context.User.Id);

                StringBuilder result = new StringBuilder();

                foreach (var prop in typeof(SkillSheet).GetProperties())
                {
                    if (prop.Name.Equals("CharacterId") || prop.Name.Equals("Id"))
                        continue;
                    result.Append(prop.Name + ": " + prop.GetValue(c.Skills) + "\n");
                }

                var embed = Util.EmbedTool.BuildBasicEmbed("Skills for " + Context.User.Username, result.ToString());

                await ReplyAsync(embed: embed);
            }


            [Command("set")]
            [Alias("set")]
            [Summary("Set a new character's Skills based on SPECIAL and Tag!")]
            public async Task SetSkillsAsync(string tag1, string tag2, string tag3)
            {
                var user = Context.User;
                var character = _charService.GetCharacter(user.Id);

                if (character == null)
                {
                    await ReplyAsync(String.Format(Messages.ERR_CHAR_NOT_FOUND, user.Mention));
                    return;
                }
                if (character.Skills.Barter != 0) // check Barter if its zero, if it is, then they haven't set a special...
                {
                    await ReplyAsync(String.Format(Messages.ERR_SKILLS_ALREADYSET, user.Mention));
                    return;
                }
                if (!_falloutService.IsValidSpecial(character.Special))
                {
                    await ReplyAsync(String.Format(Messages.ERR_SPECIAL_INVALID, user.Mention));
                    return;
                }

                character.Skills = _falloutService.CalculateInitialSkills(character.Special, character);

                try
                {
                    character.Skills = _falloutService.TagSkills(character.Skills, tag1, tag2, tag3);
                }
                catch (ArgumentException e)
                {
                    await ReplyAsync(String.Format(e.Message, user.Mention));
                    return;
                }

                await _charService.SaveCharacterAsync(character);
                await ReplyAsync(String.Format(Messages.CHAR_SKILLS_SETSUCCESS, user.Mention));
            }
            [Command("add")]
            public async Task AddSkillPointsAsync(string skillName, int points)
            {
                var character = _charService.GetCharacter(Context.User.Id);

                if (character == null)
                {
                    await ReplyAsync(String.Format(Messages.ERR_CHAR_NOT_FOUND, Context.User.Mention));
                    return;
                }
                if (character.Skills.Barter == 0)
                {
                    await ReplyAsync(String.Format(Messages.ERR_SKILLS_NOTSET, Context.User.Mention));
                    return;
                }
                if (character.SkillPoints < points)
                {
                    await ReplyAsync(String.Format(Messages.ERR_SKILLS_NOTENOUGH, Context.User.Mention));
                    return;
                }

                foreach (var prop in typeof(SkillSheet).GetProperties())
                {
                    if (prop.Name.Equals("CharacterId"))
                        continue;
                    if (prop.Name.ToLower().Equals(skillName.ToLower())) // case insensitivity
                    {
                        prop.SetValue(character.Skills, (int)prop.GetValue(character.Skills) + points);
                    }
                }
            }
        }
    }

    [Group("roll")]
    [Alias("r")]
    public class FalloutRollModule : ModuleBase<SocketCommandContext>
    {
        public FalloutService _falloutService { get; set; }
        public CharacterService _charService { get; set; }

        [Command("strength")]
        [Alias("str")]
        public async Task RollStrength() { await ReplyAsync(GetSpRoll(Context.User, "Strength")); }
        [Command("perception")]
        [Alias("per")]
        public async Task RollPerception() { await ReplyAsync(GetSpRoll(Context.User, "Perception")); }
        [Command("endurance")]
        [Alias("end")]
        public async Task RollEndurance() { await ReplyAsync(GetSpRoll(Context.User, "Endurance")); }
        [Command("charisma")]
        [Alias("cha")]
        public async Task RollCharisma() { await ReplyAsync(GetSpRoll(Context.User, "Charisma")); }
        [Command("intelligence")]
        [Alias("int")]
        public async Task RollIntelligence() { await ReplyAsync(GetSpRoll(Context.User, "Intelligence")); }
        [Command("agility")]
        [Alias("agl")]
        public async Task RollAgility() { await ReplyAsync(GetSpRoll(Context.User, "Agility")); }
        [Command("luck")]
        [Alias("luc")]
        public async Task RollLuck() { await ReplyAsync(GetSpRoll(Context.User, "Luck")); }

        [Command("barter")]
        public async Task RollBarter() { await ReplyAsync(GetSkillRoll(Context.User, "Barter")); }
        [Command("energy weapons")]
        [Alias("energyweapons")]
        public async Task RollEnergyWeapons() { await ReplyAsync(GetSkillRoll(Context.User, "EnergyWeapons")); }
        [Command("explosives")]
        public async Task RollExplosives() { await ReplyAsync(GetSkillRoll(Context.User, "Explosives")); }
        [Command("guns")]
        public async Task RollGuns() { await ReplyAsync(GetSkillRoll(Context.User, "Guns")); }
        [Command("lockpick")]
        public async Task RollLockpick() { await ReplyAsync(GetSkillRoll(Context.User, "Lockpick")); }
        [Command("medicine")]
        public async Task RollMedicine() { await ReplyAsync(GetSkillRoll(Context.User, "Medicine")); }
        [Command("meleeweapons")]
        [Alias("melee weapons")]
        public async Task RollMeleeWeapons() { await ReplyAsync(GetSkillRoll(Context.User, "MeleeWeapons")); }
        [Command("repair")]
        public async Task RollRepair() { await ReplyAsync(GetSkillRoll(Context.User, "Repair")); }
        [Command("science")]
        public async Task RollScience() { await ReplyAsync(GetSkillRoll(Context.User, "Science")); }
        [Command("sneak")]
        public async Task RollSneak() { await ReplyAsync(GetSkillRoll(Context.User, "Sneak")); }
        [Command("speech")]
        public async Task RollSpeech() { await ReplyAsync(GetSkillRoll(Context.User, "Speech")); }
        [Command("survival")]
        public async Task RollSurvival() { await ReplyAsync(GetSkillRoll(Context.User, "Survival")); }
        [Command("unarmed")]
        public async Task RollUnarmed() { await ReplyAsync(GetSkillRoll(Context.User, "Unarmed")); }

        private string GetSpRoll(IUser user, String specialToRoll)
        {
            var character = _charService.GetCharacter(user.Id);

            if (character == null)
            {
                return String.Format(Messages.ERR_CHAR_NOT_FOUND, user.Mention);
            }
            if (!_falloutService.IsValidSpecial(character.Special))
            {
                return String.Format(Messages.ERR_SPECIAL_INVALID, user.Mention);
            }
            return _falloutService.GetSpecialRollResult(specialToRoll, character);
        }
        private string GetSkillRoll(IUser user, String skillToRoll)
        {
            var character = _charService.GetCharacter(user.Id);

            if (character == null)
            {
                return String.Format(Messages.ERR_CHAR_NOT_FOUND, user.Mention);
            }
            if (!_falloutService.IsValidSpecial(character.Special))
            {
                return String.Format(Messages.ERR_SPECIAL_INVALID, user.Mention);
            }
            if (character.Skills.Barter == 0)
            {
                return String.Format(Messages.ERR_SKILLS_NOTSET, user.Mention);
            }
            return _falloutService.GetSkillRollResult(skillToRoll, character);
        }
    }
}
