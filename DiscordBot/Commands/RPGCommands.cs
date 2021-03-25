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
		private readonly IProfileService _profileService;

		public RPGCommands(IItemService itemService, IMapService mapService, IProfileService profileService)
		{
			_itemService = itemService;
			_mapService = mapService;
			_profileService = profileService;
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
					map.Tiles.Add(new Tile { PosX = x, PosY = y, Graphic = @"Sprites\grass.bmp" });
				}

			await ctx.Channel.SendMessageAsync("map created").ConfigureAwait(false);

			await _mapService.CreateNewMapAsync(map).ConfigureAwait(false);
		}

		[Command("spawn")]
		public async Task Spawn(CommandContext ctx)
        {
			Profile profile = await _profileService.GetOrCreateProfileAsync(ctx.Member.Id).ConfigureAwait(false);
			await Spawn(ctx, profile).ConfigureAwait(false);
		}
		public async Task Spawn(CommandContext ctx, Profile profile)
		{
			Tile tile = await _mapService.GetTileByCoords(0, 0).ConfigureAwait(false);
			await _mapService.SetPositionAsync(profile, tile).ConfigureAwait(false);
		}

		[Command("look")]
		public async Task Look(CommandContext ctx, int range)
		{
			Profile profile = await _profileService.GetOrCreateProfileAsync(ctx.Member.Id).ConfigureAwait(false);

			await Look(ctx, range, profile).ConfigureAwait(false);
		}
		public async Task Look(CommandContext ctx, int range, Profile profile)
		{
			// Make sure player has a tile
			if (profile.CurrentTile == null){
				await Spawn(ctx, profile).ConfigureAwait(false);
			}

			await GetTiles(ctx,
				profile.CurrentTile.PosX - range,
				profile.CurrentTile.PosX + range,
				profile.CurrentTile.PosY - range,
				profile.CurrentTile.PosY + range)
				.ConfigureAwait(false);
		}

		// replace this with reactions
		[Command("go")]
		public async Task Go(CommandContext ctx, string direction)
		{
			Profile profile = await _profileService.GetOrCreateProfileAsync(ctx.Member.Id).ConfigureAwait(false);

			// Make sure player has a tile
			if (profile.CurrentTile == null){
				await Spawn(ctx, profile).ConfigureAwait(false);
			}

			Tile tile = null;
			if(direction == "north")
            {
				tile = await _mapService.GetTileByCoords(profile.CurrentTile.PosX, profile.CurrentTile.PosY - 1).ConfigureAwait(false);
            }
			else if (direction == "south")
			{
				tile = await _mapService.GetTileByCoords(profile.CurrentTile.PosX, profile.CurrentTile.PosY + 1).ConfigureAwait(false);

			}
			else if (direction == "east")
			{
				tile = await _mapService.GetTileByCoords(profile.CurrentTile.PosX + 1, profile.CurrentTile.PosY).ConfigureAwait(false);
			}
			else if (direction == "west")
			{
				tile = await _mapService.GetTileByCoords(profile.CurrentTile.PosX - 1, profile.CurrentTile.PosY).ConfigureAwait(false);
			}

			if (tile != null){
				await _mapService.SetPositionAsync(profile, tile).ConfigureAwait(false);
			}

			await Look(ctx, 5, profile).ConfigureAwait(false);
		}

		[Command("gettiles")]
		public async Task GetTiles(CommandContext ctx, int xMin, int xMax, int yMin, int yMax)
		{
			Tile[] tiles = await _mapService.GetTilesByConstraint(xMin, xMax, yMin, yMax);
			Tile[,] tiles2d = new Tile[xMax - xMin + 1, yMax - yMin +1];

			// array to 2d array
			foreach (Tile tile in tiles)
            {
				tiles2d[tile.PosX - xMin, tile.PosY - yMin] = tile;
            }
			Bitmap bitmap = CreateImage(tiles2d);
			Stream stream = BitmapToStream(bitmap);

			// Sends the message
			await new DiscordMessageBuilder()
				.WithContent("very cool")
				.WithFile("map.png", stream)
				.SendAsync(ctx.Channel);
		}

		public Stream BitmapToStream(Bitmap bitmap)
        {
			// Transform bitmap to a byte array
			ImageConverter converter = new ImageConverter();
			byte[] bytes = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));

			// Turns the bytearray into a memoryStream. A stream is required to send the message
			Stream stream = new MemoryStream(bytes);
			return stream;
		}

		public Bitmap CreateImage(Tile[,] tiles)
		{
			int tileWidth = 16;
			int tileHeight = 16;

			// Create the graphical representation of the map based on the tiles
			Bitmap bitmap = new Bitmap(tiles.GetLength(0) * tileWidth, tiles.GetLength(1) * tileHeight);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				for (int x = 0; x < tiles.GetLength(0); x++)
					for (int y = 0; y < tiles.GetLength(1); y++)
					{
						if (tiles[x, y] != null)
                        {
							g.DrawImage(Image.FromFile($@"{tiles[x, y].Graphic}"), x * tileWidth, y * tileHeight);
						}
                        else
                        {
							g.DrawImage(Image.FromFile(@"Sprites\devtex.bmp"), x * tileWidth, y * tileHeight);
						}
					}
			}
			return bitmap;
		}
	}
}
