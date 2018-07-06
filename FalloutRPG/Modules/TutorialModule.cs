using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    public class TutorialModule : InteractiveBase<SocketCommandContext>
    {
        [Command("tutorial")]
        [Alias("tut", "guide")]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task ShowTutorialAsync()
        {
            var userInfo = Context.User;
            var message = new StringBuilder();

            var embed = EmbedTool.BuildBasicEmbedWithFields("Command: !tutorial", 
                "More functionality will be added in future iterations.", Pages.TUTORIAL_TITLES, Pages.TUTORIAL_CONTENTS);

            await Context.User.SendMessageAsync(userInfo.Mention, embed: embed);
            await Context.User.SendMessageAsync("Note: Use !help if you get stuck.");
        }
    }
}
