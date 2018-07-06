using Discord.Commands;
using FalloutRPG.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    public class GamblingModule : ModuleBase<SocketCommandContext>
    {
        private readonly GamblingService _gamblingService;

        public GamblingModule(GamblingService gamblingService)
        {
            _gamblingService = gamblingService;
        }

        [Command("balance")]
        [Alias("bal")]
        public async Task ViewBalanceAsync()
        {
            var user = Context.User;
            await ReplyAsync("U HAV: " + _gamblingService.UserBalances[user]);
        }
    }
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
            await ReplyAsync( _crapsService.PlaceBet(Context.User, betType, betAmount));
        }
        [Command("info")]
        public async Task GetGameInfoAsync()
        {
            await ReplyAsync(_crapsService.SpewGutsOut());
        }
    }
    //[Group("slots")]
    //public class SlotsModule : ModuleBase<SocketCommandContext>
    //{
    //    private readonly GamblingService _gamblingService;

    //    public SlotsModule(GamblingService gamblingService)
    //    {
    //        _gamblingService = gamblingService;
    //    }

    //    [Command("test")]
    //    public async Task TestAsync()
    //    {
    //        _gamblingService.Test();
    //        await ReplyAsync(_gamblingService.TestInt.ToString());
    //    }
    //}
}
