using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using FalloutRPG.Constants;
using FalloutRPG.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    [Group("help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task ShowHelpAsync()
        {
            var userInfo = Context.User;
            var embed = EmbedTool.BuildBasicEmbed("Command: !help",
                "**!help character** - Displays character help menu.\n" +
                "**!help roll** - Displays roll help menu.");

            await ReplyAsync(userInfo.Mention, embed: embed);
        }

        [Group("character")]
        public class CharacterHelpModule : InteractiveBase<SocketCommandContext>
        {
            [Command]
            public async Task ShowCharacterHelpAsync()
            {
                var page1 = PageTool.BuildPageWithFields("Command: !help character", 
                    PageTool.CreatePageFields(Messages.HELP_CHAR_PAGE1_TITLES, Messages.HELP_CHAR_PAGE1_CONTENTS));

                var page2 = PageTool.BuildPageWithFields("Command: !help character", 
                    PageTool.CreatePageFields(Messages.HELP_CHAR_PAGE2_TITLES, Messages.HELP_CHAR_PAGE2_CONTENTS));

                var pager = PageTool.BuildPaginatedMessage(new[] { page1, page2 });

                await PagedReplyAsync(pager, new ReactionList
                {
                    Forward = true,
                    Backward = true,
                    Jump = false,
                    Trash = true
                });
            }
        }
    }
}
