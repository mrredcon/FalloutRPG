using Discord;

namespace FalloutRPG.Util
{
    public class EmbedTool
    {
        public static Embed BuildBasicEmbed(string title, string content)
        {
            // The embed description has a max of 2048 characters
            content = StringTool.Truncate(content, 2048);

            var builder = new EmbedBuilder()
                .WithDescription(content)
                .WithColor(new Color(0, 128, 255))
                .WithAuthor(author => {
                    author
                        .WithName(title);
                });

            return builder.Build();
        }
    }
}
