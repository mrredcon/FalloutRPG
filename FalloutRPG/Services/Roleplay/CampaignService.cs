using Discord;
using Discord.WebSocket;
using FalloutRPG.Constants;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Services.Roleplay
{
    public class CampaignService
    {
        private readonly DiscordSocketClient _client;
        private readonly PlayerService _playerService;

        private readonly IRepository<Player> _playerRepository;

        public CampaignService(PlayerService playerService, IRepository<Player> playerRepository, DiscordSocketClient client)
        {
            _playerService = playerService;
            _playerRepository = playerRepository;
            _client = client;
        }

        public async Task CreateCampaignAsync(string name, SocketGuild guild, Player player)
        {
            if (player.Campaign != null)
                throw new Exception(Exceptions.CAMP_TOO_MANY);
            if (guild.TextChannels.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).Count() > 0)
                throw new Exception(Exceptions.CAMP_NAME_NOT_UNIQUE);

            var role = await guild.CreateRoleAsync($"{name} Campaigner");

            var channel = await guild.CreateTextChannelAsync($"{name}-campaign");

            // sets permissions so only the bot and the campaigners can see their own channel
            await channel.AddPermissionOverwriteAsync(guild.EveryoneRole, new OverwritePermissions(viewChannel: PermValue.Deny));
            await channel.AddPermissionOverwriteAsync(role, new OverwritePermissions(viewChannel: PermValue.Allow));
            await channel.AddPermissionOverwriteAsync(_client.CurrentUser, new OverwritePermissions(viewChannel: PermValue.Allow, manageChannel: PermValue.Allow));

            await guild.GetUser(player.DiscordId).AddRoleAsync(role);

            player.Campaign = new Campaign(name, player.DiscordId, role.Id, channel.Id);
            await _playerService.SavePlayerAsync(player);
        }

        public async Task<int> CountAllMembersAsync(Campaign camp) =>
            await _playerRepository.Query.Where(x => x.Campaign.Equals(camp)).CountAsync();

        public async Task<List<Player>> GetAllMembersAsync(Campaign camp) =>
            await _playerRepository.Query.Where(x => x.Campaign.Equals(camp)).ToListAsync();

        public async Task DeleteCampaignAsync(Campaign campaign, SocketGuild guild)
        {
            foreach (var player in await GetAllMembersAsync(campaign))
            {
                player.Campaign = null;
                await _playerService.SavePlayerAsync(player);
            }

            await guild.GetRole(campaign.RoleId).DeleteAsync();
            await guild.GetChannel(campaign.TextChannelId).DeleteAsync();
        }
    }
}
