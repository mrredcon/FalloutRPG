using Discord.Addons.Interactive;
using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Util;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    [Group("help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task ShowHelpAsync()
        {
            var userInfo = Context.User;
            var embed = EmbedTool.BuildBasicEmbed("Command: !help",
                "**!help character** - Displays character help menu.\n" +
                "**!help roll** - Displays roll help menu.\n" +
                "**!help skills** - Displays a list of skills.\n\n" +
                $"*Note: All commands have a {Globals.RATELIMIT_SECONDS} second cooldown.*");

            await ReplyAsync(userInfo.Mention, embed: embed);
        }

        [Group("character")]
        public class CharacterHelpModule : InteractiveBase<SocketCommandContext>
        {
            [Command]
            [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task ShowCharacterHelpAsync()
            {
                var page1 = PageTool.BuildPageWithFields("Command: !help character", 
                    PageTool.CreatePageFields(Pages.HELP_CHAR_PAGE1_TITLES, Pages.HELP_CHAR_PAGE1_CONTENTS));

                var page2 = PageTool.BuildPageWithFields("Command: !help character", 
                    PageTool.CreatePageFields(Pages.HELP_CHAR_PAGE2_TITLES, Pages.HELP_CHAR_PAGE2_CONTENTS));

                var pager = PageTool.BuildPaginatedMessage(new[] { page1, page2 }, Context.User);

                await PagedReplyAsync(pager, new ReactionList
                {
                    Forward = true,
                    Backward = true,
                    Jump = false,
                    Trash = true
                });
            }
        }

        [Group("roll")]
        public class RollHelpModule : InteractiveBase<SocketCommandContext>
        {
            [Command]
            [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task ShowRollHelpAsync()
            {
                var page1 = PageTool.BuildPageWithFields("Command: !help roll",
                    PageTool.CreatePageFields(Pages.HELP_ROLL_PAGE1_TITLES, Pages.HELP_ROLL_PAGE1_CONTENTS));

                var pager = PageTool.BuildPaginatedMessage(new[] { page1 }, Context.User);

                await PagedReplyAsync(pager, new ReactionList
                {
                    Forward = true,
                    Backward = true,
                    Jump = false,
                    Trash = true
                });
            }
        }

        [Group("skills")]
        public class SkillsHelpModule : InteractiveBase<SocketCommandContext>
        {
            [Command]
            [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task ShowSkillsHelpAsync()
            {
                var userInfo = Context.User;
                var message = new StringBuilder();

                foreach (var skill in Globals.SKILL_NAMES)
                    message.Append($"{skill}\n");

                var embed = EmbedTool.BuildBasicEmbed("!help skills", message.ToString());

                await ReplyAsync(userInfo.Mention, embed: embed);
            }
        }
    }
}
