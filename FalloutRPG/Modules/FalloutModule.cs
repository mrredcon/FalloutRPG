using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Models;
using FalloutRPG.Services;
using System;
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
        public async Task ViewSpecial()
        {
            var character = _charService.GetCharacter(Context.User.Id);

            await ReplyAsync("Strength: " + character.Special.Strength);
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
                await Context.Channel.SendMessageAsync(string.Format(Constants.Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                return;
            }

            if (character.Special != null)
            {
                await ReplyAsync(String.Format(Constants.Messages.ERR_SPECIAL_EXISTS, userInfo.Mention));
                return;
            }

            Special special = _falloutService.ParseSpecialString(newSpecial);

            if (special != null)
            {
                // Success condition
                if (_falloutService.IsValidSpecial(special, newChar: true))
                {
                    character.Special = special;
                    await _falloutService.SaveSpecial(character, special);
                    await _charService.SaveCharacterAsync(character);
                    await ReplyAsync(String.Format(Constants.Messages.CHAR_SPECIAL_SUCCESS, userInfo.Mention));
                }
                else
                    await ReplyAsync(String.Format(Constants.Messages.ERR_SPECIAL_INVALID, userInfo.Mention));
            }
            else
                await ReplyAsync(String.Format(Constants.Messages.ERR_SPECIAL_PARSE, userInfo.Mention));
        }
    }
}
