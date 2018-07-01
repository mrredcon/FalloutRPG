using Discord.Commands;
using FalloutRPG.Addons;
using System;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    public class FalloutModule : ModuleBase<SocketCommandContext>
    {
        [Command("daysleft"), Ratelimit(1, 0.1, Measure.Minutes)]
        [Alias("countdown", "days")]
        [Summary("Displays how many days are left until the release of Fallout 76 on 14th November 2018")]
        public async Task DaysLeftAsync()
        {
            DateTime today = DateTime.Now;
            DateTime release = new DateTime(2018, 11, 14);
            TimeSpan span = (release - today);

            await Context.Channel.SendMessageAsync($"There are {span.Days} days, {span.Hours} hours, {span.Minutes} minutes, {span.Seconds} seconds and {span.Milliseconds} milliseconds left until release! (UTC)");
        }

        [RequireOwner]
        [Command("echo")]
        public async Task EchoAsync(string input)
        {
            await Context.Channel.SendMessageAsync(input);
        }
    }
}
