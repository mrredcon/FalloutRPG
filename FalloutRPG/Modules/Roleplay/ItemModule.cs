using Discord;
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
        [Group("create")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public class ItemCreateModule : ModuleBase<SocketCommandContext>
        {
            private readonly ItemService _itemService;

            public ItemCreateModule(ItemService itemService)
            {
                _itemService = itemService;
            }

            [Command("misc")]
            public async Task CreateMiscItemAsync(string name, string desc, int value, double weight)
            {
                await _itemService.CreateMiscItemAsync(name, desc, value, weight);
            }
        }

        private readonly CharacterService _charService;
        private readonly ItemService _itemService;

        public ItemModule(CharacterService charService, ItemService itemService)
        {
            _charService = charService;
            _itemService = itemService;
        }

        [Command("print")]
        [RequireOwner]
        public async Task PrintItems(IUser user)
        {
            var character = await _charService.GetCharacterAsync(user.Id);

            if (character == null)
                return;

            StringBuilder sb = new StringBuilder();

            foreach (var item in character.Inventory)
                sb.Append(item.Name + " ");

            await ReplyAsync(sb.ToString());
        }

        [Command("give")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddItem(IUser user, string itemName)
        {
            var item = await _itemService.GetItemAsync(itemName);
            var character = await _charService.GetCharacterAsync(user.Id);

            if (item == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_ITEM_NOT_FOUND, Context.User.Mention));
                return;
            }
            if (character == null)
            {
                await ReplyAsync(String.Format(Messages.ERR_CHAR_NOT_FOUND, user.Mention));
                return;
            }

            character.Inventory.Add(item);
            await _charService.SaveCharacterAsync(character);

            await ReplyAsync(String.Format(Messages.ITEM_GIVE_SUCCESS, item.Name, character.Name, Context.User.Mention));
        }
    }
}
