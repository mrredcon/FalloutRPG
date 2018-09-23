using Discord.Commands;
using FalloutRPG.Constants;
using FalloutRPG.Services.Roleplay;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    [Group("item"), Alias("item")]
    public class ItemModule : ModuleBase<SocketCommandContext>
    {
        [Group("create"), Alias("add"), RequireUserPermission(Discord.GuildPermission.Administrator)]
        public class ItemCreateModule : ModuleBase<SocketCommandContext>
        {
            private readonly ItemService _itemService;

            public ItemCreateModule(ItemService itemService)
            {
                _itemService = itemService;
            }

            [Command("misc")]
            public async Task CreateMiscItemAsync(string name)
            {
                await _itemService.CreateMiscItem(name);
            }
        }
    }
}
