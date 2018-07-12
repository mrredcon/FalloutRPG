using Discord;
using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Helpers;
using FalloutRPG.Services.Roleplay;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    public class RoleplayModule : ModuleBase<SocketCommandContext>
    {
        private readonly CharacterService _charService;
        private readonly ExperienceService _expService;

        public RoleplayModule(
            CharacterService charService,
            ExperienceService expService)
        {
            _charService = charService;
            _expService = expService;
        }

        [Command("pay")]
        public async Task PayAsync(IUser user, int amount)
        {
            if (amount < 1) return;

            var sourceChar = await _charService.GetCharacterAsync(Context.User.Id);
            if (sourceChar == null) return;

            var targetChar = await _charService.GetCharacterAsync(user.Id);
            if (targetChar == null)
            {
                await ReplyAsync(string.Format(Messages.ERR_CHAR_NOT_FOUND, Context.User.Mention));
                return;
            }

            if (amount > sourceChar.Money)
            {
                await ReplyAsync(string.Format(Messages.ERR_NOT_ENOUGH_CAPS, Context.User.Mention));
                return;
            }

            sourceChar.Money -= amount;
            targetChar.Money += amount;

            await _charService.SaveCharacterAsync(sourceChar);
            await _charService.SaveCharacterAsync(targetChar);

            await ReplyAsync(string.Format(Messages.PAY_SUCCESS, user.Mention, amount, Context.User.Mention));
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

            var embed = EmbedHelper.BuildBasicEmbed("Command: $highscores", strBuilder.ToString());

            await ReplyAsync(userInfo.Mention, embed: embed);
        }
    }
}
