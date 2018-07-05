using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
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
    public class StatsModule : InteractiveBase<SocketCommandContext>
    {
        [Group("special")]
        [Alias("spec", "sp")]
        public class CharacterSpecialModule : ModuleBase<SocketCommandContext>
        {
            private readonly CharacterService _charService;
            private readonly StatsService _statsService;

            public CharacterSpecialModule(CharacterService charService, StatsService statsService)
            {
                _charService = charService;
                _statsService = statsService;
            }

            [Command]
            [Alias("show")]
            public async Task ShowSpecialAsync(IUser targetUser = null)
            {
                var userInfo = Context.User;
                var character = targetUser == null
                    ? _charService.GetCharacter(userInfo.Id)
                    : _charService.GetCharacter(targetUser.Id);

                if (character == null)
                {
                    await Context.Channel.SendMessageAsync(
                        string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                if (!_statsService.IsSpecialSet(character))
                {
                    await Context.Channel.SendMessageAsync(
                        string.Format(Messages.ERR_DESC_NOT_FOUND, userInfo.Mention));
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

                await Context.Channel.SendMessageAsync(userInfo.Mention, embed: embed);
            }

            [Command("set")]
            public async Task SetSpecialAsync(int str, int per, int end, int cha, int inte, int agi, int luc)
            {
                var userInfo = Context.User;
                var character = _charService.GetCharacter(userInfo.Id);
                var special = new int[] { str, per, end, cha, inte, agi, luc };

                if (character == null)
                {
                    await Context.Channel.SendMessageAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                if (_statsService.IsSpecialSet(character))
                {
                    await Context.Channel.SendMessageAsync(string.Format(Messages.ERR_SPECIAL_EXISTS, userInfo.Mention));
                    return;
                }

                try
                {
                    await _statsService.SetInitialSpecialAsync(character, special);
                    await Context.Channel.SendMessageAsync(string.Format(Messages.CHAR_SPECIAL_SUCCESS, userInfo.Mention));
                }
                catch (Exception e)
                {
                    await Context.Channel.SendMessageAsync(e.Message);
                }
            }
        }

        [Group("skills")]
        [Alias("skill", "sk")]
        public class CharacterSkillsModule : ModuleBase<SocketCommandContext>
        {
            private readonly CharacterService _charService;
            private readonly StatsService _statsService;

            public CharacterSkillsModule(CharacterService charService, StatsService statsService)
            {
                _charService = charService;
                _statsService = statsService;
            }

            [Command]
            [Alias("show")]
            public async Task ShowSkillsAsync(IUser targetUser = null)
            {
                var userInfo = Context.User;
                var character = targetUser == null
                    ? _charService.GetCharacter(userInfo.Id)
                    : _charService.GetCharacter(targetUser.Id);

                if (character == null)
                {
                    await Context.Channel.SendMessageAsync(
                        string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                if (!_statsService.AreSkillsSet(character))
                {
                    await Context.Channel.SendMessageAsync(
                        string.Format(Messages.ERR_SKILLS_NOTSET, userInfo.Mention));
                    return;
                }

                var embed = EmbedTool.BuildBasicEmbed("Command: !character skills",
                    $"**Name:** {character.FirstName} {character.LastName}\n" +
                    $"**Barter:** {character.Skills.Barter}\n" +
                    $"**Energy Weapons:** {character.Skills.EnergyWeapons}\n" +
                    $"**Explosives:** {character.Skills.Explosives}\n" +
                    $"**Guns:** {character.Skills.Guns}\n" +
                    $"**Lockpick:** {character.Skills.Lockpick}\n" +
                    $"**Medicine:** {character.Skills.Medicine}\n" +
                    $"**MeleeWeapons:** {character.Skills.MeleeWeapons}\n" +
                    $"**Repair:** {character.Skills.Repair}\n" +
                    $"**Science:** {character.Skills.Science}\n" +
                    $"**Sneak:** {character.Skills.Sneak}\n" +
                    $"**Speech:** {character.Skills.Speech}\n" +
                    $"**Survival:** {character.Skills.Survival}\n" +
                    $"**Unarmed:** {character.Skills.Unarmed}");

                await Context.Channel.SendMessageAsync(userInfo.Mention, embed: embed);
            }

            [Command("set")]
            public async Task SetSkillsAsync(string tag1, string tag2, string tag3)
            {
                var userInfo = Context.User;
                var character = _charService.GetCharacter(userInfo.Id);

                if (character == null)
                {
                    await Context.Channel.SendMessageAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                if (_statsService.AreSkillsSet(character))
                {
                    await Context.Channel.SendMessageAsync(string.Format(Messages.ERR_SKILLS_ALREADYSET, userInfo.Mention));
                    return;
                }

                try
                {
                    await _statsService.SetTagSkills(character, tag1, tag2, tag3);
                    await Context.Channel.SendMessageAsync(string.Format(Messages.CHAR_SKILLS_SETSUCCESS, userInfo.Mention));
                }
                catch (Exception e)
                {
                    await Context.Channel.SendMessageAsync(e.Message);
                }
            }
        }
    }
}
