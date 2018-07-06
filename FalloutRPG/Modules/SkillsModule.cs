using Discord;
using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Services;
using FalloutRPG.Util;
using System;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    [Group("character")]
    [Alias("char")]
    public class SkillsModule : ModuleBase<SocketCommandContext>
    {
        [Group("skills")]
        [Alias("skill", "sk")]
        public class CharacterSkillsModule : ModuleBase<SocketCommandContext>
        {
            private readonly CharacterService _charService;
            private readonly SkillsService _skillsService;

            public CharacterSkillsModule(CharacterService charService, SkillsService skillsService)
            {
                _charService = charService;
                _skillsService = skillsService;
            }

            [Command]
            [Alias("show", "view")]
            [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task ShowSkillsAsync(IUser targetUser = null)
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

                if (!_skillsService.AreSkillsSet(character))
                {
                    await ReplyAsync(
                        string.Format(Messages.ERR_SKILLS_NOT_FOUND, userInfo.Mention));
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
                    $"**Unarmed:** {character.Skills.Unarmed}\n" +
                    $"*You have {character.SkillPoints} left to spend! (!char skills spend)*");

                await ReplyAsync(userInfo.Mention, embed: embed);
            }

            [Command("set")]
            [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task SetSkillsAsync(string tag1, string tag2, string tag3)
            {
                var userInfo = Context.User;
                var character = _charService.GetCharacter(userInfo.Id);

                if (character == null)
                {
                    await ReplyAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                if (_skillsService.AreSkillsSet(character))
                {
                    await ReplyAsync(string.Format(Messages.ERR_SKILLS_ALREADY_SET, userInfo.Mention));
                    return;
                }

                try
                {
                    await _skillsService.SetTagSkills(character, tag1, tag2, tag3);
                    await ReplyAsync(string.Format(Messages.SKILLS_SET_SUCCESS, userInfo.Mention));
                }
                catch (Exception e)
                {
                    await ReplyAsync($"{Messages.FAILURE_EMOJI} {e.Message} ({userInfo.Mention})");
                }
            }

            [Command("spend")]
            [Alias("put")]
            [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task SpendSkillPointsAsync(string skill, int points)
            {
                var userInfo = Context.User;
                var character = _charService.GetCharacter(userInfo.Id);

                if (character == null)
                {
                    await ReplyAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                if (!_skillsService.AreSkillsSet(character))
                {
                    await ReplyAsync(string.Format(Messages.ERR_SKILLS_NOT_FOUND, userInfo.Mention));
                    return;
                }

                try
                {
                    _skillsService.PutPointsInSkill(character, skill, points);
                    await ReplyAsync(Messages.SKILLS_SPEND_POINTS_SUCCESS);
                }
                catch (Exception e)
                {
                    await ReplyAsync($"{Messages.FAILURE_EMOJI} {e.Message} ({userInfo.Mention})");
                }
            }
        }
    }
}
