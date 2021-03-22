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
using System.IO;
using System.Text;
using System.Threading;
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
			await _itemService.CreateNewItemAsync(new Item { Name = name, Description = description });
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

			await _mapService.CreateNewMapAsync(map).ConfigureAwait(false);
		}



		[Command("createimage")]
		public async Task CreateImage(CommandContext ctx, int size = 5)
		{

			// Limits size to 100 (The image can be maximum (100^2 * 16^2) = 2,560,000 pixels. Expected max filesize of 141 kB)
			size = size > 100 ? 100 : size;

			int tileWidth = 16;
			int tileHeight = 16;

			Bitmap[] sprites = new Bitmap[] {
				new Bitmap(Image.FromFile(@"Sprites\devtex.bmp")),
				new Bitmap(Image.FromFile(@"Sprites\grass.bmp"))
			};

			var random = new Random();

			Bitmap[,] tiles = new Bitmap[size, size];

			// Populate placeholder map
			for (int x = 0; x < tiles.GetLength(0); x++)
				for (int y = 0; y < tiles.GetLength(1); y++)
				{
					// Get random image for the tile
					var rIndex = random.Next(0, sprites.Length);

					tiles[x, y] = sprites[rIndex];
				}

			// Create the graphical representation of the map based on the tiles
			Bitmap bitmap = new Bitmap(tiles.GetLength(0) * tileWidth, tiles.GetLength(1) * tileHeight);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				for (int x = 0; x < tiles.GetLength(0); x++)
					for (int y = 0; y < tiles.GetLength(1); y++)
					{
						g.DrawImage(tiles[x, y], x * tileWidth, y * tileHeight);
					}
			}

			// Transform bitmap to a byte array
			ImageConverter converter = new ImageConverter();
			byte[] bytes = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));

			// Turns the bytearray into a memoryStream. A stream is required to send the message
			Stream stream = new MemoryStream(bytes);

			// Sends the message
			await new DiscordMessageBuilder()
				.WithContent("Nice map, Gordon")
				.WithFile("map.png", stream)
				.SendAsync(ctx.Channel);
		}
	}
}
