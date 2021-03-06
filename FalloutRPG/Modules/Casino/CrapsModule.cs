﻿using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Services;
using FalloutRPG.Services.Casino;
using System;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Casino
{
    [Group("craps")]
    [Alias("cr")]
    [Ratelimit(Globals.RATELIMIT_TIMES, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
    public class CrapsModule : ModuleBase<SocketCommandContext>
    {
        private readonly GamblingService _gamblingService;
        private readonly CrapsService _crapsService;
        private readonly HelpService _helpService;

        public CrapsModule(
            GamblingService gamblingService,
            CrapsService crapsService,
            HelpService helpService)
        {
            _gamblingService = gamblingService;
            _crapsService = crapsService;
            _helpService = helpService;
        }

        [Command]
        [Alias("help")]
        public async Task ShowCrapsHelpAsync()
        {
            await _helpService.ShowCrapsHelpAsync(Context);
        }

        [Command("join")]
        public async Task JoinCrapsGameAsync()
        {
            if (_gamblingService.IsGamblingEnabledChannel(Context.Channel.Id))
            {
                var result = _crapsService.JoinMatch(Context.User, Context.Channel).Result;

                if (result == CrapsService.JoinMatchResult.Success)
                {
                    await ReplyAsync(String.Format(Messages.CRAPS_JOIN_MATCH, Context.User.Mention));
                }
                else if (result == CrapsService.JoinMatchResult.NewMatch)
                {
                    await ReplyAsync(String.Format(Messages.CRAPS_NEW_MATCH, Context.User.Mention));
                }
                else if (result == CrapsService.JoinMatchResult.AlreadyInMatch)
                {
                    await ReplyAsync(String.Format(Messages.CRAPS_ALREADY_IN_MATCH, Context.User.Mention));
                }
                else if (result == CrapsService.JoinMatchResult.NullCharacter)
                {
                    await ReplyAsync(String.Format(Messages.ERR_CHAR_NOT_FOUND, Context.User.Mention));
                }
                else
                {
                    await ReplyAsync(String.Format(Messages.ERR_CRAPS_JOIN_FAIL, Context.User.Mention));
                }
            }
        }

        [Command("leave")]
        [Alias("quit")]
        public async Task LeaveCrapsGameAsync()
        {
            if (_gamblingService.IsGamblingEnabledChannel(Context.Channel.Id))
            {
                if (await _crapsService.LeaveMatch(Context.User))
                    await ReplyAsync(String.Format(Messages.CRAPS_LEAVE_MATCH, Context.User.Mention));
                else
                    await ReplyAsync(String.Format(Messages.ERR_CRAPS_LEAVE_FAIL, Context.User.Mention));
            }
        }

        [Command("roll")]
        [Alias("r")]
        public async Task RollAsync()
        {
            if (_gamblingService.IsGamblingEnabledChannel(Context.Channel.Id))
            {
                string rollResult = await _crapsService.Roll(Context.User);
                await ReplyAsync(rollResult);
            }
        }

        [Command("bet")]
        [Alias("b")]
        public async Task BetAsync(string betType, int betAmount)
        {
            if (_gamblingService.IsGamblingEnabledChannel(Context.Channel.Id))
                await ReplyAsync(_crapsService.PlaceBet(Context.User, betType, betAmount));
        }

        [Command("pass")]
        public async Task PassDice()
        {
            if (_gamblingService.IsGamblingEnabledChannel(Context.Channel.Id))
            {
                if (_crapsService.Shooter == Context.User)
                {
                    if (_crapsService.PassDice())
                    {
                        await ReplyAsync(String.Format(Messages.CRAPS_NEW_SHOOTER, _crapsService.Shooter.Mention));
                    }
                    else
                    {
                        await ReplyAsync(String.Format(Messages.ERR_CRAPS_PASS_FAIL, Context.User.Mention));
                    }
                }
            }
        }
    }
}
