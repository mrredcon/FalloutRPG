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
        private readonly CharacterService _charService;
        private readonly FalloutService _falloutService;

        public FalloutCharacterModule(
            CharacterService charService,
            FalloutService falloutService)
        {
            _charService = charService;
            _falloutService = falloutService;
        }

        [Command("viewspecial")]
        public async Task ViewSpecialAsync()
        {
            var c = _charService.GetCharacter(Context.User.Id);

            StringBuilder result = new StringBuilder();

            foreach (var prop in typeof(Special).GetProperties())
            {
                if (prop.Name.Equals("CharacterId"))
                    continue;
                result.Append(prop.Name + ": " + prop.GetValue(c.Special) + "\n");
            }

            var embed = Util.EmbedTool.BuildBasicEmbed("S.P.E.C.I.A.L. stats for " + Context.User.Username, result.ToString());

            await ReplyAsync(embed: embed);
        }
        [Command("viewskills")]
        public async Task ViewSkillsAsync()
        {
            var c = _charService.GetCharacter(Context.User.Id);

            StringBuilder result = new StringBuilder();

            foreach (var prop in typeof(SkillSheet).GetProperties())
            {
                if (prop.Name.Equals("CharacterId"))
                    continue;
                result.Append(prop.Name + ": " + prop.GetValue(c.Skills) + "\n");
            }

            var embed = Util.EmbedTool.BuildBasicEmbed("S.P.E.C.I.A.L. stats for " + Context.User.Username, result.ToString());

            await ReplyAsync(embed: embed);
        }

        [Command("setspecial")]
        [Alias("setsp")]
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
        [Command("setskills")]
        [Alias("setsk")]
        [Summary("Set a new character's Skills based on SPECIAL and Tag!")]
        public async Task SetCharacterSkills(string tag1, string tag2, string tag3)
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
    }
}
