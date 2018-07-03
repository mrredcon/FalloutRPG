using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Exceptions;
using FalloutRPG.Services;
using FalloutRPG.Util;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    [Group("character")]
    [Alias("char")]
    public class CharacterModule : InteractiveBase<SocketCommandContext>
    {
        private readonly CharacterService _charService;
        private readonly ExperienceService _expService;

        private const int RATELIMIT_SECONDS = 3;

        public CharacterModule(
            CharacterService charService,
            ExperienceService expService)
        {
            _charService = charService;
            _expService = expService;
        }

        [Command]
        [Alias("help")]
        [Ratelimit(1, RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task ShowCommandHelpAsync()
        {
            var embed = EmbedTool.BuildBasicEmbed("Command: !character",
                "**!character show** - Displays your character.\n" +
                "**!character show [@user]** - Displays specified user's character.\n" +
                "**!character create [forename] [surname]** - Creates your character.\n" +
                "**!character story** - Displays your character's story.\n" +
                "**!character story [@user]** - Displays specified user's character story.\n" +
                "**!character story update [story]** - Updates your character's story.\n" +
                "**!character desc** - Displays your character's description.\n" +
                "**!character desc [@user]** - Displays specified user's character description.\n" +
                "**!character desc update [desc]** - Updates your character's description.");

            await Context.Channel.SendMessageAsync(string.Empty, embed: embed);
        }

        [Command("show")]
        [Alias("display")]
        [Ratelimit(1, RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task ShowCharacterAsync(IUser targetUser = null)
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

            var level = _expService.CalculateLevelForExperience(character.Experience);

            var embed = EmbedTool.BuildBasicEmbed($"{character.FirstName} {character.LastName}",
                $"**Description:** {character.Description}\n" +
                $"**Story:** {character.Story}\n" +
                $"**Experience:** {character.Experience}\n" +
                $"**Level:** {level}");

            await Context.Channel.SendMessageAsync(userInfo.Mention, embed: embed);
        }

        [Command("create")]
        [Alias("new")]
        [Ratelimit(1, RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task CreateCharacterAsync(string firstName, string lastName)
        {
            var userInfo = Context.User;

            try
            {
                await _charService.CreateCharacterAsync(userInfo.Id, firstName, lastName);
                await Context.Channel.SendMessageAsync(
                    string.Format(Messages.CHAR_CREATED_SUCCESS, userInfo.Mention));
            }
            catch (CharacterException ex)
            {
                await Context.Channel.SendMessageAsync(string.Format(ex.Message, userInfo.Mention));
                return;
            }
        }

        [Command("highscores")]
        [Alias("hiscores", "high", "hi", "highscore", "hiscore")]
        [Ratelimit(1, RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task ShowHighScoresAsync()
        {
            var userInfo = Context.User;
            var charList = await _charService.GetHighScoresAsync();
            var strBuilder = new StringBuilder();

            for (var i = 0; i < charList.Count; i++)
            {
                var level = _expService.CalculateLevelForExperience(charList[i].Experience);
                var user = Context.Guild.GetUser(charList[i].DiscordId);

                strBuilder.Append(
                    $"**{i + 1}:** {charList[i].FirstName} {charList[i].LastName}" +
                    $" - Level: {level}" +
                    $" - Experience: {charList[i].Experience}" +
                    $" - User: {user.Username}");
            }

            var embed = EmbedTool.BuildBasicEmbed("!command highscores", strBuilder.ToString());

            await Context.Channel.SendMessageAsync(userInfo.Mention, embed: embed);
        }

        [Group("stats")]
        [Alias("statistics", "level", "levels", "experience", "exp")]
        public class CharacterStatsModule : ModuleBase<SocketCommandContext>
        {
            private readonly CharacterService _charService;
            private readonly ExperienceService _expService;

            public CharacterStatsModule(CharacterService charService, ExperienceService expService)
            {
                _charService = charService;
                _expService = expService;
            }

            [Command]
            [Alias("show")]
            [Ratelimit(1, RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task ShowCharacterStatsAsync(IUser targetUser = null)
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

                var level = _expService.CalculateLevelForExperience(character.Experience);
                var expToNextLevel = _expService.CalculateRemainingExperienceToNextLevel(character.Experience);

                var embed = EmbedTool.BuildBasicEmbed("!character stats",
                    $"**Name:** {character.FirstName} {character.LastName}\n" +
                    $"**Level:** {level}\n" +
                    $"**Experience:** {character.Experience}\n" +
                    $"**To Next Level:** {expToNextLevel}");

                await Context.Channel.SendMessageAsync(userInfo.Mention, embed: embed);
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
            [Ratelimit(1, RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task ShowCharacterStoryAsync(IUser targetUser = null)
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

                if (character.Story == null || character.Story.Equals(""))
                {
                    await Context.Channel.SendMessageAsync(
                        string.Format(Messages.ERR_STORY_NOT_FOUND, userInfo.Mention));
                    return;
                }

                var embed = EmbedTool.BuildBasicEmbed("Command: !character story",
                    $"**Name:** {character.FirstName} {character.LastName}\n" +
                    $"**Story:** {character.Story}");

                await Context.Channel.SendMessageAsync(userInfo.Mention, embed: embed);
            }

            [Command("update")]
            [Alias("set")]
            [Ratelimit(1, RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task UpdateCharacterStoryAsync([Remainder]string story)
            {
                var userInfo = Context.User;
                var character = _charService.GetCharacter(userInfo.Id);

                if (character == null)
                {
                    await Context.Channel.SendMessageAsync(
                        string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                character.Story = story;

                await _charService.SaveCharacterAsync(character);
                await Context.Channel.SendMessageAsync(
                    string.Format(Messages.CHAR_STORY_SUCCESS, userInfo.Mention));
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
            [Ratelimit(1, RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task ShowCharacterDescriptionAsync(IUser targetUser = null)
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

                if (character.Description == null || character.Description.Equals(""))
                {
                    await Context.Channel.SendMessageAsync(
                        string.Format(Messages.ERR_DESC_NOT_FOUND, userInfo.Mention));
                    return;
                }

                var embed = EmbedTool.BuildBasicEmbed("Command: !character story",
                    $"**Name:** {character.FirstName} {character.LastName}\n" +
                    $"**Description:** {character.Description}");

                await Context.Channel.SendMessageAsync(userInfo.Mention, embed: embed);
            }

            [Command("update")]
            [Alias("set")]
            [Ratelimit(1, RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task UpdateCharacterDescriptionAsync([Remainder]string description)
            {
                var userInfo = Context.User;
                var character = _charService.GetCharacter(userInfo.Id);

                if (character == null)
                {
                    await Context.Channel.SendMessageAsync(
                        string.Format(Messages.ERR_CHAR_NOT_FOUND, userInfo.Mention));
                    return;
                }

                character.Description = description;

                await _charService.SaveCharacterAsync(character);
                await Context.Channel.SendMessageAsync(
                    string.Format(Messages.CHAR_DESC_SUCCESS, userInfo.Mention));
            }
        }
    }
}
