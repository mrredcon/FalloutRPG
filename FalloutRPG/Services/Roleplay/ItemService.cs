using FalloutRPG.Data.Repositories;
using FalloutRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Services.Roleplay
{
    public class ItemService
    {
        private readonly IRepository<Item> _itemRepo;
        //private readonly IRepository<ItemAmmo> _ammoRepository;
        //private readonly IRepository<ItemApparel> _apparelRepository;
        //private readonly IRepository<ItemConsumable> _itemConsumable;
        //private readonly IRepository<ItemMisc> _itemMisc;
        //private readonly IRepository<ItemWeapon> _itemWeapon;

        public ItemService(IRepository<Item> itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public async Task CreateMiscItem(string name)
        {
            var newItem = new ItemMisc
            {
                Name = name
            };

            await _itemRepo.AddAsync(newItem);
        }
    }
}
