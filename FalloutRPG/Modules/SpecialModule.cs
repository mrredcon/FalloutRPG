using Discord;
using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Services;
using FalloutRPG.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    [Group("character")]
    [Alias("char")]
    public class SpecialModule : ModuleBase<SocketCommandContext>
    {
        [Group("special")]
        [Alias("spec", "sp")]
        public class CharacterSpecialModule : ModuleBase<SocketCommandContext>
        {
            private readonly CharacterService _charService;
            private readonly SpecialService _specService;

            public CharacterSpecialModule(CharacterService charService, SpecialService specService)
            {
                _charService = charService;
                _specService = specService;
            }

            [Command]
            [Alias("show")]
            [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task ShowSpecialAsync(IUser targetUser = null)
            {
                var userInfo = Context.User;
                var character = targetUser == null
                    ? _charService.GetCharacter(userInfo.Id)
                    : _charService.GetCharacter(targetUser.Id);

                if (character == null)
                {
                    await ReplyAsync(
                        string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                if (!_specService.IsSpecialSet(character))
                {
                    await ReplyAsync(
                        string.Format(Messages.ERR_SPECIAL_NOT_FOUND, userInfo.Mention));
                    return;
                }

                var embed = EmbedTool.BuildBasicEmbed("Command: !character special",
                    $"**Name:** {character.FirstName} {character.LastName}\n" +
                    $"**STR:** {character.Special.Strength}\n" +
                    $"**PER:** {character.Special.Perception}\n" +
                    $"**END:** {character.Special.Endurance}\n" +
                    $"**CHA:** {character.Special.Charisma}\n" +
                    $"**INT:** {character.Special.Intelligence}\n" +
                    $"**AGI:** {character.Special.Agility}\n" +
                    $"**LUC:** {character.Special.Luck}\n");

                await ReplyAsync(userInfo.Mention, embed: embed);
            }

            [Command("set")]
            [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task SetSpecialAsync(int str, int per, int end, int cha, int inte, int agi, int luc)
            {
                var userInfo = Context.User;
                var character = _charService.GetCharacter(userInfo.Id);
                var special = new int[] { str, per, end, cha, inte, agi, luc };

                if (character == null)
                {
                    await ReplyAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                if (_specService.IsSpecialSet(character))
                {
                    await ReplyAsync(string.Format(Messages.ERR_SPECIAL_ALREADY_SET, userInfo.Mention));
                    return;
                }

                try
                {
                    await _specService.SetInitialSpecialAsync(character, special);
                    await ReplyAsync(string.Format(Messages.SPECIAL_SET_SUCCESS, userInfo.Mention));
                }
                catch (Exception e)
                {
                    await ReplyAsync($"{Messages.FAILURE_EMOJI} {e.Message} ({userInfo.Mention})");
                }
            }
        }
    }
}
