using DiscordBot.Models.Items;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public interface IItemService
    {
        Task CreateNewItemAsync(Item item);
        Task<Item> GetItemByName(string itemName);
    }

    public class ItemService : IItemService
    {
        private readonly RPGContext _context;
        public ItemService(RPGContext context)
        {
            _context = context;
        }

        public async Task CreateNewItemAsync(Item item)
        {
            await _context.AddAsync(item).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Item> GetItemByName(string itemName)
        {
            itemName = itemName.ToLower();

            return await _context.Items
                .FirstOrDefaultAsync(x => x.Name.ToLower() == itemName);
        }
    }
}
