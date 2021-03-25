using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Services
{
    public interface IMapService
    {
        Task CreateNewMapAsync(Map map);
        Task<Tile> GetTileByCoords(int CoordX, int CoordY);
        Task<Tile[]> GetTilesByConstraint(int xMin, int xMax, int yMin, int yMax);
        Task SetPositionAsync(TileEntity entity, Tile tile);
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

        public async Task<Tile> GetTileByCoords(int CoordX, int CoordY)
        {
            return await _context.Tiles
                .FirstOrDefaultAsync(tile => tile.PosX == CoordX && tile.PosY == CoordY);
        }

        public async Task<Tile[]> GetTilesByConstraint(int xMin, int xMax, int yMin, int yMax)
        {
            var data = _context.Tiles.Where(tile => tile.PosX >= xMin && tile.PosX <= xMax && tile.PosY >= yMin && tile.PosY <= yMax);
            Tile[] arr = await data.ToArrayAsync();
            return arr;
        }


        public async Task SetPositionAsync(TileEntity entity, Tile tile)
        {
            entity.CurrentTile = tile;

            _context.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            // This is neccesary to prevent an exception on next Update
            _context.Entry(entity).State = EntityState.Detached;
            _context.Entry(tile).State = EntityState.Detached;

            return;
        }
    }
}
