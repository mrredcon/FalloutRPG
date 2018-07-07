using Discord.Commands;
using FalloutRPG.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    [Group("craps")]
    public class CrapsModule : ModuleBase<SocketCommandContext>
    {
        private readonly CrapsService _crapsService;

        public CrapsModule(CrapsService crapsService)
        {
            _crapsService = crapsService;
        }

        [Command("join")]
        public async Task JoinCrapsGameAsync()
        {
            _crapsService.JoinMatch(Context.User);
            await ReplyAsync("Joined match!");
        }
        [Command("roll")]
        public async Task RollAsync()
        {
            string rollResult = _crapsService.Roll(Context.User);
            await ReplyAsync(rollResult);
        }
        [Command("bet")]
        public async Task BetAsync(string betType, int betAmount)
        {
            await ReplyAsync(_crapsService.PlaceBet(Context.User, betType, betAmount));
        }
        [Command("info")]
        public async Task GetGameInfoAsync()
        {
            await ReplyAsync(_crapsService.SpewGutsOut());
        }
    }
}
