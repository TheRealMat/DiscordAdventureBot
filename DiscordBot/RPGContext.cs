﻿using DiscordBot.Models.Items;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot
{
    public class RPGContext : DbContext
    {
        public RPGContext(DbContextOptions<RPGContext> options) : base(options) { }
        public DbSet<Item> Items { get; set; }
        public DbSet<Map> maps { get; set; }
        public DbSet<Tile> tiles { get; set; }
    }
}
