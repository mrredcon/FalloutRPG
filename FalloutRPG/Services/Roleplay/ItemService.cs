using FalloutRPG.Data.Repositories;
using FalloutRPG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Services.Roleplay
{
    public class ItemService
    {
        private readonly IRepository<Item> _itemRepo;

        public ItemService(IRepository<Item> itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public async Task CreateMiscItemAsync(string name, string desc, int value, double weight)
        {
            var newItem = new ItemMisc
            {
                Name = name,
                Description = desc,
                Value = value,
                Weight = weight
            };

            await _itemRepo.AddAsync(newItem);
        }

        public async Task<Item> GetItemAsync(string name) =>
            await _itemRepo.Query.Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();
    }
}
