using Discord.Addons.Interactive;
using Discord.Commands;
using FalloutRPG.Constants;
using FalloutRPG.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class HelpService
    {
        private readonly InteractiveService _interactiveService;

        public HelpService(InteractiveService interactiveService)
        {
            _interactiveService = interactiveService;
        }

        #region General Help
        public async Task ShowHelpAsync(SocketCommandContext context)
        {
            var userInfo = context.User;
            var embed = EmbedTool.BuildBasicEmbed("Command: !help",
                "**!help character** - Displays character help menu.\n" +
                "**!help roll** - Displays roll help menu.\n" +
                "**!help skills** - Displays a list of skills.\n\n" +
                $"*Note: All commands have a {Globals.RATELIMIT_SECONDS} second cooldown.*");

            await context.Channel.SendMessageAsync(userInfo.Mention, embed: embed);
        }
        #endregion

        #region Character Help
        public async Task ShowCharacterHelpAsync(SocketCommandContext context)
        {
            var page1 = PageTool.BuildPageWithFields("Command: !help character",
                PageTool.CreatePageFields(Pages.HELP_CHAR_PAGE1_TITLES, Pages.HELP_CHAR_PAGE1_CONTENTS));

            var page2 = PageTool.BuildPageWithFields("Command: !help character",
                PageTool.CreatePageFields(Pages.HELP_CHAR_PAGE2_TITLES, Pages.HELP_CHAR_PAGE2_CONTENTS));

            var pager = PageTool.BuildPaginatedMessage(new[] { page1, page2 }, context.User);

            await _interactiveService.SendPaginatedMessageAsync(context, pager, new ReactionList
            {
                Forward = true,
                Backward = true,
                Jump = false,
                Trash = true
            });
        }
        #endregion

        #region Roll Help
        public async Task ShowRollHelpAsync(SocketCommandContext context)
        {
            var page1 = PageTool.BuildPageWithFields("Command: !help roll",
                PageTool.CreatePageFields(Pages.HELP_ROLL_PAGE1_TITLES, Pages.HELP_ROLL_PAGE1_CONTENTS));

            var pager = PageTool.BuildPaginatedMessage(new[] { page1 }, context.User);

            await _interactiveService.SendPaginatedMessageAsync(context, pager, new ReactionList
            {
                Forward = true,
                Backward = true,
                Jump = false,
                Trash = true
            });
        }
        #endregion

        #region Skills Help
        public async Task ShowSkillsHelpAsync(SocketCommandContext context)
        {
            var userInfo = context.User;
            var message = new StringBuilder();

            foreach (var skill in Globals.SKILL_NAMES)
                message.Append($"{skill}\n");

            var embed = EmbedTool.BuildBasicEmbed("!help skills", message.ToString());

            await context.Channel.SendMessageAsync(userInfo.Mention, embed: embed);
        }
        #endregion
    }
}
