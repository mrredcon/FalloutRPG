using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Helpers;
using FalloutRPG.Services.Roleplay;
using System;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    public class FalloutModule : ModuleBase<SocketCommandContext>
    {
        private readonly CharacterService _charService;
        private readonly ExperienceService _expService;

        public FalloutModule(CharacterService charService, ExperienceService expService)
        {
            _charService = charService;
            _expService = expService;
        }

        [Command("highscores")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        [Alias("hiscores", "high", "hi", "highscore", "hiscore")]
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
                    $" | Level: {level}" +
                    $" | Experience: {charList[i].Experience}" +
                    $" | User: {user.Username}\n");
            }

            var embed = EmbedHelper.BuildBasicEmbed("Command: !highscores", strBuilder.ToString());

            await ReplyAsync(userInfo.Mention, embed: embed);
        }

        [Command("daysleft")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        [Alias("countdown", "days")]
        public async Task DaysLeftAsync()
        {
            var today = DateTime.Now;
            var release = new DateTime(2018, 11, 14);
            var span = (release - today);

            await ReplyAsync(
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
            await ReplyAsync(input);
        }
    }
}