using Discord.Commands;
using FalloutRPG.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    //class GamblingModule
    //{
    //}
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
            if (rollResult == null || rollResult.Equals(""))
                await ReplyAsync("Failed to roll (Are you the shooter?)");
            else
                await ReplyAsync(rollResult);
        }
        [Command("bet")]
        public async Task BetAsync(string betType, int betAmount)
        {
            bool result = _crapsService.PlaceBet(Context.User, betType, betAmount);
            if (result)
            {
                await ReplyAsync("Placed bet!");
            }
            else
                await ReplyAsync("Failed to place bet!");
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
