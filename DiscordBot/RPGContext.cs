using DiscordBot.Models.Items;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot
{
    public class RPGContext : DbContext
    {
        RPGContext()
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public RPGContext(DbContextOptions<RPGContext> options) : base(options) { }
        public DbSet<Item> Items { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<Tile> Tiles { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<TileEntity> TileEntities { get; set; }
    }
}
