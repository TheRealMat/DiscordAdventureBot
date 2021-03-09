using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DiscordBot.Database;

namespace DiscordBot.Commands
{
	class TestCommands : BaseCommandModule
	{
		/* SQLite START */
		[Command("sqlget")]
		[Description("Returns a message")]
		public async Task SQLGet(CommandContext ctx)
		{
			var items = SQLiteDatabaseAccess.LoadEntities();
			string output = "";

			foreach (var item in items)
			{
				output += $"({item.DiscordID}) Name: {item.Name} Age: {item.Age}\n";
			}

			await ctx.Channel.SendMessageAsync(output).ConfigureAwait(false);
		}

		[Command("sqlAdd")]
		[Description("Returns a message")]
		public async Task SQLAdd(CommandContext ctx, int age)
		{
			var e = new Entity();
			e.DiscordID = ctx.User.Id.ToString();
			e.Name = ctx.User.Username;
			e.Age = age;

			SQLiteDatabaseAccess.SaveEntity(e);
		}

		[Command("removeMe")]
		[Description("Removed you from the sql database")]
		public async Task SQLRemoveMe(CommandContext ctx)
		{
			string id = ctx.User.Id.ToString();

			Console.WriteLine($"Removing a user {id}");
			SQLiteDatabaseAccess.RemoveMe(ctx.User.Id.ToString());
		}

		/* SQL END */

		[Command("test")]
		[Description("Returns a message")]
		public async Task Test(CommandContext ctx)
		{
			await ctx.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/759192495782625292/818433424628121651/video0_-_2021-03-08T233110.495.mp4").ConfigureAwait(false);
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
		[Description("Rolls custom dice")]
		public async Task Roll(CommandContext ctx, [Description("Die face amount")] int dieFaces = 6, [Description("Die amount (max 100)")] int numberOfDie = 1)
		{

			// Will not throw more than 100 dices
			if (numberOfDie > 100)
			{
				numberOfDie = 100;
			}

			Random random = new Random();

			string diceNotation = $"{numberOfDie}d{dieFaces}";
			string message = $"Rolling {diceNotation} die\n[";
			int total = 0;

			// Throws each dice and sums the values
			for (int i = 0; i < numberOfDie; i++)
			{
				int diceroll = random.Next(1, dieFaces + 1);
				total += diceroll;
				message += diceroll.ToString();
				// Formatting. The last ',' will be omitted from the output. Will instead get a ']' to symbolize end of the rolls
				message += i != numberOfDie - 1 ? ", " : "]";
			}


			// Prints the total
			message += $"\n\nTotal: {total}";

			await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
		}

		[Command("roll")]
		[Description("Rolls custom dice")]
		public async Task Roll(CommandContext ctx, [Description("Die notation")] string diceNotation = "1d6")
		{

			diceNotation = diceNotation.ToLower();

			// Matches <digit>d<digit> i.e. that this '1d6' pattern is found
			Regex rx = new Regex(@"^\d+d\d+$",
					RegexOptions.Compiled | RegexOptions.IgnoreCase);


			MatchCollection matches = rx.Matches(diceNotation);

			// No matches so exit
			if (matches.Count == 0)
				return;

			// Basic: We only care about the first one
			string match = matches[0].Value;

			string[] diceData = match.Split("d");

			// No need for try catch. Input has already been verified
			int numberOfDie = int.Parse(diceData[0]);
			int dieFaces = int.Parse(diceData[1]);

			await Roll(ctx, dieFaces, numberOfDie);
		}


		/* Wormhole section */
		Queue<string> WormholeQueue = new Queue<string>();

		[Command("wormhole")]
		[Description("Wormhole service")]
		public async Task Wormhole(CommandContext ctx)
		{

			if (WormholeQueue.Count == 0)
				return;

			string message = WormholeQueue.Dequeue();

			await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
		}

		[Command("wormhole")]
		public async Task Wormhole(CommandContext ctx, [Description("The message that will be sent into the wormhole")] params string[] message)
		{
			WormholeQueue.Enqueue(string.Join(" ", message));
		}
	}
}
