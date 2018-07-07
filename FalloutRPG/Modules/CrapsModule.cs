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
        private readonly GamblingService _gamblingService;
        private readonly CrapsService _crapsService;

        public CrapsModule(GamblingService gamblingService, CrapsService crapsService)
        {
            _gamblingService = gamblingService;
            _crapsService = crapsService;
        }

        [Command("join")]
        public async Task JoinCrapsGameAsync()
        {
            if (_gamblingService.IsGamblingEnabledChannel(Context.Channel.Id))
            {
                _crapsService.JoinMatch(Context.User);
                await ReplyAsync("Joined match!");
            }
        }
        [Command("roll")]
        public async Task RollAsync()
        {
            if (_gamblingService.IsGamblingEnabledChannel(Context.Channel.Id))
            {
                string rollResult = _crapsService.Roll(Context.User);
                await ReplyAsync(rollResult);
            }
        }
        [Command("bet")]
        public async Task BetAsync(string betType, int betAmount)
        {
            if (_gamblingService.IsGamblingEnabledChannel(Context.Channel.Id))
                await ReplyAsync(_crapsService.PlaceBet(Context.User, betType, betAmount));
        }
        [Command("info")]
        public async Task GetGameInfoAsync()
        {
            //await ReplyAsync(_crapsService.SpewGutsOut());
        }
    }
}
