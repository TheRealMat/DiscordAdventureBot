using DiscordBot.Models.Items;
using DiscordBot.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class RPGCommands : BaseCommandModule
    {
		private readonly IItemService _itemService;

		public RPGCommands(IItemService itemService)
		{
			_itemService = itemService;
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
	}
}
