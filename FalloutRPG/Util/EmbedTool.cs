using Discord;

namespace FalloutRPG.Util
{
    public class EmbedTool
    {
        /// <summary>
        /// Builds a simple Embed with a title and content.
        /// </summary>
        /// <remarks>
        /// The embed description has a max of 2048 characters.
        /// </remarks>
        public static Embed BuildBasicEmbed(string title, string content)
        {
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
