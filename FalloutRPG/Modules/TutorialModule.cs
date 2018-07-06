using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
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
        public async Task ShowTutorialAsync()
        {
            var userInfo = Context.User;
            var message = new StringBuilder();

            var fieldTitles = new string[]
            {
                "STEP 1: CREATING A CHARACTER",
                "STEP 2: SETTING A STORY AND DESCRIPTION",
                "STEP 3: SETTING A SPECIAL",
                "STEP 4: SETTING TAG SKILLS",
                "STEP 5: ROLLING"
            };

            var fieldContents = new string[]
            {
                "Use !char create [firstname] [lastname] to create your character.",
                "Use !char story set [story] and !char desc set [desc] to set your story and description.",
                "Use !char spec set [S] [P] [E] [C] [I] [A] [L] to set your SPECIAL.",
                "Use !char skills set [tag1] [tag2] [tag3] to set tag skills.",
                "Use !roll [special] and !roll [skill] to roll."
            };

            var embed = EmbedTool.BuildBasicEmbedWithFields("Command: !tutorial", 
                "More functionality will be added in future iterations.", fieldTitles, fieldContents);

            await Context.User.SendMessageAsync(userInfo.Mention, embed: embed);
            await Context.User.SendMessageAsync("Note: Use !help if you get stuck.");
        }
    }
}
