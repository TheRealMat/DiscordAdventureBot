using DiscordBot.Models;
using DiscordBot.Models.Items;
using DiscordBot.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class RPGCommands : BaseCommandModule
    {
		private readonly IItemService _itemService;
		private readonly IMapService _mapService;

		public RPGCommands(IItemService itemService, IMapService mapService)
		{
			_itemService = itemService;
			_mapService = mapService;
		}

		[Command("createitem")]
		public async Task CreateItem(CommandContext ctx, string name, string description)
		{
			await _itemService.CreateNewItemAsync(new Item {Name = name, Description = description });
		}

		[Command("iteminfo")]
		public async Task ItemInfo(CommandContext ctx, string name)
		{
			var item = await _itemService.GetItemByName(name).ConfigureAwait(false);

			if (item == null)
            {
				await ctx.Channel.SendMessageAsync($"There is no item called{name}");
				return;
            }

			await ctx.Channel.SendMessageAsync($"Name: {item.Name}, Description: {item.Description}");
		}

		[Command("createmap")]
		public async Task CreateMap(CommandContext ctx, int size)
		{
			Map map = new Map();
			Tile[,] tiles = new Tile[size, size];

			//generate
			for (int x = 0; x < tiles.GetLength(0); x++)
				for (int y = 0; y < tiles.GetLength(1); y++)
                {
					// is it possible to add this to the map instead of using a one dimensional collection?
					tiles[x, y] = new Tile { PosX = x, PosY = y, graphic = "<:powerlich:818391341163348008>" };

					map.Tiles.Add(new Tile { PosX = x, PosY = y, graphic = "<:powerlich:818391341163348008>" });
                }

			//print
			string message = "";
			for (int x = 0; x < tiles.GetLength(0); x++)
            {
				for (int y = 0; y < tiles.GetLength(1); y++)
				{
					message += tiles[x, y].graphic;
				}
				message += Environment.NewLine;
			}
			message += "test";
			await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);



			//await _mapService.CreateNewMapAsync(map).ConfigureAwait(false);


		}

		[Command("createimage")]
		public async Task CreateImage(CommandContext ctx, int size)
		{
			int tileWidth = 16;
			int tileHeight = 16;

			Bitmap image1 = new Bitmap(Image.FromFile(@"Sprites\devtex.bmp"));

			Bitmap[,] tiles = new Bitmap[size, size];

			// populate
			for (int x = 0; x < tiles.GetLength(0); x++)
				for (int y = 0; y < tiles.GetLength(1); y++)
                {
					tiles[x, y] = image1;
                }



			Bitmap bitmap = new Bitmap(tiles.GetLength(0) * tileWidth, tiles.GetLength(1) * tileHeight);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				for (int x = 0; x < tiles.GetLength(0); x++)
					for (int y = 0; y < tiles.GetLength(1); y++)
					{
						g.DrawImage(tiles[x, y], x * tileWidth, y * tileHeight);
					}
			}

			await ctx.Channel.SendMessageAsync("a").ConfigureAwait(false);

		}
	}
}
