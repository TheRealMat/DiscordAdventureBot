using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Models;

namespace DiscordBot.Services
{
    public interface IMapService
    {
        Task CreateNewMapAsync(Map map);
    }
    public class MapService : IMapService
    {
        private readonly RPGContext _context;
        public MapService(RPGContext context)
        {
            _context = context;
        }

        public async Task CreateNewMapAsync(Map map)
        {
            await _context.AddAsync(map).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
