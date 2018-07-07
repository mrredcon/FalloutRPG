using Discord;
using Discord.Commands;
using FalloutRPG.Addons;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("info"), Ratelimit(1, 0.1, Measure.Minutes)]
        public async Task InfoAsync()
        {
            var buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
            var app = await Context.Client.GetApplicationInfoAsync();

            var mrredUser = Context.Client.GetUser(129306645548367872UL);
            var dukeUser = Context.Client.GetUser(409676326262538240UL);

            var builder = new EmbedBuilder()
                .WithTitle("Fallout Roleplay Automated Game System")
                .WithDescription("A Fallout-based roleplaying bot designed for the Country Road Bar.")
                .WithColor(new Color(0, 128, 255)) // Blue
                .WithFooter(footer =>
                {
                    footer
                        .WithText($"Version {Assembly.GetExecutingAssembly().GetName().Version.ToString()} | " +
                            $"Last updated {String.Format("{0:d-MMM-yy}", buildDate)}");
                })
                .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl().ToString())
                .WithAuthor(author => {
                    author
                        .WithName("F.R.A.G.S.")
                        .WithIconUrl(Context.Client.CurrentUser.GetAvatarUrl().ToString());
                })
                .AddField("Developed By", $"{app.Owner.Mention} and {mrredUser.Mention}")
                .AddField("Name and Artwork By", $"{dukeUser.Mention}")
                .AddField("Library", $"Discord.Net ({DiscordConfig.Version})")
                .AddField("Runtime", $"{RuntimeInformation.FrameworkDescription} {RuntimeInformation.ProcessArchitecture} " +
                    $"({RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture})")
                .AddField("Uptime", GetUptime().ToString())
                .AddField("Heap Size", $"{GetHeapSize()}MiB")
                .AddField("Servers", Context.Client.Guilds.Count.ToString())
                .AddField("Users", Context.Client.Guilds.Sum(g => g.Users.Count));
            var embed = builder.Build();

            await Context.Channel.SendMessageAsync(string.Empty, embed: embed).ConfigureAwait(false);
        }

        private static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
    }
}
