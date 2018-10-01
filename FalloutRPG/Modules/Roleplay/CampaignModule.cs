using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using FalloutRPG.Constants;
using FalloutRPG.Services;
using FalloutRPG.Services.Roleplay;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    [Group("campaign")]
    [Alias("cam", "camp")]
    [RequireBotPermission(Discord.GuildPermission.ManageRoles | Discord.GuildPermission.ManageChannels)]
    public class CampaignModule : InteractiveBase<SocketCommandContext>
    {
        private readonly CampaignService _campaignService;
        private readonly PlayerService _playerService;

        public CampaignModule(CampaignService campaignService, PlayerService playerService)
        {
            _campaignService = campaignService;
            _playerService = playerService;
        }

        [Command("create")]
        [Alias("new")]
        public async Task CreateCampaignAsync(string name)
        {
            var userInfo = Context.User;
            var player = await _playerService.GetPlayerAsync(userInfo.Id);

            try
            {
                await _campaignService.CreateCampaignAsync(name, Context.Guild, player);
                await ReplyAsync(string.Format(Messages.CAMP_CREATED_SUCCESS, userInfo.Mention));
            }
            catch (Exception e)
            {
                await ReplyAsync($"{Messages.FAILURE_EMOJI} {e.Message} ({userInfo.Mention})");
                return;
            }
        }

        [Command("add")]
        public async Task AddMemberAsync(IUser userToAdd)
        {
            var ownerInfo = Context.User;
            var campOwner = await _playerService.GetPlayerAsync(ownerInfo.Id);
            var playerToAdd = await _playerService.GetPlayerAsync(userToAdd.Id);

            if (campOwner.Campaign == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_CAMP_NOT_FOUND, ownerInfo.Mention));
                return;
            }
            if (campOwner.Campaign.OwnerId != campOwner.DiscordId)
            {
                await ReplyAsync(String.Format(Messages.ERR_CAMP_NOT_OWNER, ownerInfo.Mention));
                return;
            }
            if (playerToAdd.Campaign != null)
            {
                await ReplyAsync(String.Format(Messages.ERR_CAMP_ALREADY_IN, userToAdd.Mention));
                return;
            }

            await userToAdd.SendMessageAsync(String.Format(Messages.CAMP_INVITATION, ownerInfo.Username, campOwner.Campaign.Name, userToAdd.Mention));

            var response = await NextMessageAsync(new EnsureFromUserCriterion(userToAdd.Id));

            if (response != null && response.Content.Equals(campOwner.Campaign.Name, StringComparison.OrdinalIgnoreCase))
            {
                playerToAdd.Campaign = campOwner.Campaign;
                var role = Context.Guild.GetRole(playerToAdd.Campaign.RoleId);
                await Context.Guild.GetUser(playerToAdd.DiscordId).AddRoleAsync(role);
                await _playerService.SavePlayerAsync(playerToAdd);

                await ReplyAsync(String.Format(Messages.CAMP_JOIN_SUCCESS, userToAdd.Mention, playerToAdd.Campaign.Name));
            }
            else
            {
                await ReplyAsync(String.Format(Messages.CAMP_JOIN_FAILURE, userToAdd.Mention, playerToAdd.Campaign.Name));
            }
        }

        [Command("delete")]
        [Alias("disband", "finish", "remove", "del")]
        public async Task DeleteCampaignAsync()
        {
            var player = await _playerService.GetPlayerAsync(Context.User.Id);

            if (player.Campaign == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_CAMP_NOT_FOUND, Context.User.Mention));
                return;
            }

            if (player.Campaign.OwnerId == player.DiscordId)
            {
                int memberCount = await _campaignService.CountAllMembersAsync(player.Campaign);

                await ReplyAsync(String.Format(Messages.CAMP_REMOVE_CONFIRM, player.Campaign.Name, memberCount, Context.User.Mention));
                var response = await NextMessageAsync();

                if (response != null && response.Content.Equals(player.Campaign.Name, StringComparison.OrdinalIgnoreCase))
                {
                    await _campaignService.DeleteCampaignAsync(player.Campaign, Context.Guild);
                    await ReplyAsync(String.Format(Messages.CAMP_REMOVE_SUCCESS, Context.User.Mention));
                }
                else
                {
                    await ReplyAsync(String.Format(Messages.CAMP_NOT_REMOVED, player.Campaign.Name, Context.User.Mention));
                }
            }
            else
            {
                await ReplyAsync(String.Format(Messages.ERR_CAMP_NOT_OWNER, Context.User.Mention));
            }
        }
    }
}
