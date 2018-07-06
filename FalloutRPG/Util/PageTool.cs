using Discord;
using Discord.Addons.Interactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FalloutRPG.Util
{
    public class PageTool
    {
        public static PaginatedMessage BuildPaginatedMessage(PaginatedMessage.Page[] pages, IUser user)
        {
            var pager = new PaginatedMessage
            {
                Pages = pages,
                Color = Color.Blue,
                FooterOverride = null,
                Options = PaginatedAppearanceOptions.Default,
                Content = user.Mention
            };

            return pager;
        }

        public static PaginatedMessage.Page BuildBasicPage(string title, string content)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
                return null;

            return new PaginatedMessage.Page
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = title,
                },
                Description = content
            };
        }

        public static PaginatedMessage.Page BuildPageWithFields(string title, List<EmbedFieldBuilder> fields)
        {
            return new PaginatedMessage.Page
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = title,
                },
                Fields = fields
            };
        }

        public static EmbedFieldBuilder CreateFieldBuilder(string title, string content)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
                return null;

            return new EmbedFieldBuilder
            {
                Name = title,
                Value = content
            };
        }

        public static List<EmbedFieldBuilder> CreatePageFields(string[] titles, string[] contents)
        {
            if (titles.Length != contents.Length)
                throw new ArgumentException();

            var fields = new List<EmbedFieldBuilder>();

            for (var i = 0; i < titles.Length; i++)
                fields.Add(CreateFieldBuilder(titles[i], contents[i]));

            return fields;
        }
    }
}
