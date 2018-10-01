﻿using Discord;
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
        public async Task AddMemberAsync(IUser userToAdd, string campName)
        {
            var modInfo = Context.User;
            var campMod = await _playerService.GetPlayerAsync(modInfo.Id);

            var campaign = await _campaignService.GetCampaignAsync(campName);

            var playerToAdd = await _playerService.GetPlayerAsync(userToAdd.Id);

            if (campaign == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_CAMP_NOT_FOUND, modInfo.Mention));
                return;
            }
            if (!campaign.Moderators.Contains(campMod))
            {
                await ReplyAsync(String.Format(Messages.ERR_CAMP_NOT_MODERATOR, modInfo.Mention));
                return;
            }

            await userToAdd.SendMessageAsync(String.Format(Messages.CAMP_INVITATION, modInfo.Username, campaign.Name, userToAdd.Mention));

            var response = await NextMessageAsync(new EnsureFromUserCriterion(userToAdd.Id));

            if (response != null && response.Content.Equals(campaign.Name, StringComparison.OrdinalIgnoreCase))
            {
                campaign.Players.Add(playerToAdd);

                var role = Context.Guild.GetRole(campaign.RoleId);
                await Context.Guild.GetUser(playerToAdd.DiscordId).AddRoleAsync(role);

                await ReplyAsync(String.Format(Messages.CAMP_JOIN_SUCCESS, userToAdd.Mention, campaign.Name));
            }
            else
            {
                await ReplyAsync(String.Format(Messages.CAMP_JOIN_FAILURE, userToAdd.Mention, campaign.Name));
            }
        }

        [Command("delete")]
        [Alias("disband", "finish", "remove", "del")]
        public async Task DeleteCampaignAsync()
        {
            var player = await _playerService.GetPlayerAsync(Context.User.Id);
            var campaign = await _campaignService.GetOwnedCampaign(player);

            if (campaign == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_CAMP_NOT_FOUND, Context.User.Mention));
                return;
            }

            if (campaign.Owner.Equals(player))
            {
                int memberCount = campaign.Players.Count;

                await ReplyAsync(String.Format(Messages.CAMP_REMOVE_CONFIRM, campaign.Name, memberCount, Context.User.Mention));
                var response = await NextMessageAsync();

                if (response != null && response.Content.Equals(campaign.Name, StringComparison.OrdinalIgnoreCase))
                {
                    await _campaignService.DeleteCampaignAsync(campaign, Context.Guild);
                    await ReplyAsync(String.Format(Messages.CAMP_REMOVE_SUCCESS, Context.User.Mention));
                }
                else
                {
                    await ReplyAsync(String.Format(Messages.CAMP_NOT_REMOVED, campaign.Name, Context.User.Mention));
                }
            }
            else
            {
                await ReplyAsync(String.Format(Messages.ERR_CAMP_NOT_OWNER, Context.User.Mention));
            }
        }
    }
}