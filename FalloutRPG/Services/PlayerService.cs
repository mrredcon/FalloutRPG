using Discord.WebSocket;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FalloutRPG.Services
{
    public class PlayerService
    {
        private readonly DiscordSocketClient _client;

        private readonly IRepository<Player> _playerRepository;

        public PlayerService(IRepository<Player> playerRepository, DiscordSocketClient client)
        {
            _client = client;
            _client.UserJoined += OnUserJoined;

            _playerRepository = playerRepository;
        }

        private async Task OnUserJoined(SocketGuildUser arg) =>
            await _playerRepository.AddAsync(new Player(arg.Id));

        public async Task<Player> GetPlayerAsync(ulong discordId)
        {
            var player = await _playerRepository.Query.Where(x => x.DiscordId == discordId).Include(x => x.Campaign).FirstOrDefaultAsync();
            if (player == null) player = await AddPlayerAsync(discordId);
            return player;
        }

        /// <summary>
        /// Adds a player to the database
        /// </summary>
        /// <param name="discordId">The Discord Id of the player to add.</param>
        /// <remarks>Players should automatically be added to the database when they join a guild.</remarks>
        public async Task<Player> AddPlayerAsync(ulong discordId)
        {
            var player = new Player(discordId);
            await _playerRepository.AddAsync(player);
            return player;
        }

        public async Task SavePlayerAsync(Player playerToSave) =>
            await _playerRepository.SaveAsync(playerToSave);
    }
}
