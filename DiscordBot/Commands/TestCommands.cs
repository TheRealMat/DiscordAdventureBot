﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
	class TestCommands : BaseCommandModule
	{
		[Command("test")]
		[Description("Returns a message")]
		public async Task Test(CommandContext ctx)
		{
			await ctx.Channel.SendMessageAsync("fuck you").ConfigureAwait(false);
		}

		[Command("powerlich")]
		[Description("Returns a powerlich")]
		public async Task Powerlich(CommandContext ctx)
		{
			await ctx.Channel.SendMessageAsync("<:powerlich:818391341163348008>").ConfigureAwait(false);
		}

		[Command("emojis")]
		[Description("Returns all emojis in guild")]
		public async Task Emojis(CommandContext ctx)
		{
			IEnumerable<DiscordEmoji> emojis;
			emojis = ctx.Guild.Emojis.Values;
			string message = "";

			foreach (DiscordEmoji emoji in emojis)
			{
				message += emoji.ToString();
			}

			await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
		}

		[Command("roll")]
		[Description("rolls custom dice")]
		public async Task Roll(CommandContext ctx, [Description("Die face amount")] int dieFaces = 6, [Description("Die amount (max 100)")] int numberOfDie = 1)
		{

			// Will not throw more than 100 dices
			if (numberOfDie > 100)
			{
				numberOfDie = 100;
			}

			Random random = new Random();
			string message = "";
			int total = 0;
			for (int i = 0; i < numberOfDie; i++)
			{
				int diceroll = random.Next(1, dieFaces + 1);
				total += diceroll;
				message += diceroll.ToString();
				// Formatting. The last ',' will be omitted from the output
				if (i != numberOfDie - 1)
				{
					message += ", ";
				}
			}
			message += Environment.NewLine;
			message += "Total: ";
			message += total.ToString();

			await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
		}
	}
}
