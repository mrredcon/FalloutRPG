using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Helpers;
using FalloutRPG.Services;
using FalloutRPG.Services.Roleplay;
using System;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    [Group("character")]
    [Alias("char")]
    [Ratelimit(Globals.RATELIMIT_TIMES, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
    public class CharacterModule : InteractiveBase<SocketCommandContext>
    {
        private readonly CharacterService _charService;
        private readonly ExperienceService _expService;
        private readonly HelpService _helpService;

        public CharacterModule(
            CharacterService charService,
            ExperienceService expService,
            HelpService helpService)
        {
            _charService = charService;
            _expService = expService;
            _helpService = helpService;
        }

        [Command]
        [Alias("show", "display", "stats")]
        public async Task ShowCharacterAsync(IUser targetUser = null)
        {
            var userInfo = Context.User;
            var character = targetUser == null
                ? await _charService.GetCharacterAsync(userInfo.Id)
                : await _charService.GetCharacterAsync(targetUser.Id);

            if (character == null)
            {
                await ReplyAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                return;
            }

            var level = _expService.CalculateLevelForExperience(character.Experience);
            var expToNextLevel = _expService.CalculateRemainingExperienceToNextLevel(character.Experience);

            var description = string.IsNullOrEmpty(character.Description) ? "No description." : character.Description;
            var story = string.IsNullOrEmpty(character.Story) ? "No story." : character.Story;

            var embed = EmbedHelper.BuildBasicEmbed($"{character.FirstName} {character.LastName}",
                $"**Description:** {description}\n" +
                $"**Story:** {story}\n" +
                $"**Level:** {level}\n" +
                $"**Experience:** {character.Experience}\n" +
                $"**To Next Level:** {expToNextLevel}\n" +
                $"**Caps:** {character.Money}");

            await ReplyAsync(userInfo.Mention, embed: embed);
        }

        [Command("help")]
        [Alias("help")]
        public async Task ShowCharacterHelpAsync()
        {
            await _helpService.ShowCharacterHelpAsync(Context);
        }

        [Command("create")]
        [Alias("new")]
        public async Task CreateCharacterAsync(string firstName, string lastName)
        {
            var userInfo = Context.User;

            try
            {
                await _charService.CreateCharacterAsync(userInfo.Id, firstName, lastName);
                await ReplyAsync(string.Format(Messages.CHAR_CREATED_SUCCESS, userInfo.Mention));
            }
            catch (Exception e)
            {
                await ReplyAsync($"{Messages.FAILURE_EMOJI} {e.Message} ({userInfo.Mention})");
                return;
            }
        }

        [Group("story")]
        public class CharacterStoryModule : ModuleBase<SocketCommandContext>
        {
            private readonly CharacterService _charService;

            public CharacterStoryModule(CharacterService service)
            {
                _charService = service;
            }

            [Command]
            [Alias("show")]
            [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task ShowCharacterStoryAsync(IUser targetUser = null)
            {
                var userInfo = Context.User;
                var character = targetUser == null
                    ? await _charService.GetCharacterAsync(userInfo.Id)
                    : await _charService.GetCharacterAsync(targetUser.Id);

                if (character == null)
                {
                    await ReplyAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                if (character.Story == null || character.Story.Equals(""))
                {
                    await ReplyAsync(string.Format(Messages.ERR_STORY_NOT_FOUND, userInfo.Mention));
                    return;
                }

                var embed = EmbedHelper.BuildBasicEmbed("Command: !character story",
                    $"**Name:** {character.FirstName} {character.LastName}\n" +
                    $"**Story:** {character.Story}");

                await ReplyAsync(userInfo.Mention, embed: embed);
            }

            [Command("update")]
            [Alias("set")]
            public async Task UpdateCharacterStoryAsync([Remainder]string story)
            {
                var userInfo = Context.User;
                var character = await _charService.GetCharacterAsync(userInfo.Id);

                if (character == null)
                {
                    await ReplyAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                character.Story = story;

                await _charService.SaveCharacterAsync(character);
                await ReplyAsync(string.Format(Messages.CHAR_STORY_SUCCESS, userInfo.Mention));
            }
        }

        [Group("description")]
        [Alias("desc")]
        public class CharacterDescriptionModule : ModuleBase<SocketCommandContext>
        {
            private readonly CharacterService _charService;

            public CharacterDescriptionModule(CharacterService service)
            {
                _charService = service;
            }

            [Command]
            [Alias("show")]
            public async Task ShowCharacterDescriptionAsync(IUser targetUser = null)
            {
                var userInfo = Context.User;
                var character = targetUser == null
                    ? await _charService.GetCharacterAsync(userInfo.Id)
                    : await _charService.GetCharacterAsync(targetUser.Id);

                if (character == null)
                {
                    await ReplyAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                if (character.Description == null || character.Description.Equals(""))
                {
                    await ReplyAsync(string.Format(Messages.ERR_DESC_NOT_FOUND, userInfo.Mention));
                    return;
                }

                var embed = EmbedHelper.BuildBasicEmbed("Command: !character story",
                    $"**Name:** {character.FirstName} {character.LastName}\n" +
                    $"**Description:** {character.Description}");

                await ReplyAsync(userInfo.Mention, embed: embed);
            }

            [Command("update")]
            [Alias("set")]
            public async Task UpdateCharacterDescriptionAsync([Remainder]string description)
            {
                var userInfo = Context.User;
                var character = await _charService.GetCharacterAsync(userInfo.Id);

                if (character == null)
                {
                    await ReplyAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                character.Description = description;

                await _charService.SaveCharacterAsync(character);
                await ReplyAsync(string.Format(Messages.CHAR_DESC_SUCCESS, userInfo.Mention));
            }
        }
    }
}
