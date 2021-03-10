using DiscordBot.Models.Items;
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
		private readonly RPGContext _context;

		public RPGCommands(RPGContext context)
		{
			_context = context;
		}

		[Command("additem")]
		public async Task AddItem(CommandContext ctx, string name)
		{
			await _context.Items.AddAsync(new Item { Name = name, Description = "test description" }).ConfigureAwait(false);
			await _context.SaveChangesAsync().ConfigureAwait(false);
		}

		[Command("item")]
		public async Task Ttem(CommandContext ctx)
		{
			// doesn't print to chat. just inspect
			var items = await _context.Items.ToListAsync().ConfigureAwait(false);
		}
	}
}
