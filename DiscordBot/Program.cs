using System;
using System.Threading.Tasks;

// Invite link: https://discordapp.com/oauth2/authorize?&client_id=817714291318849558&scope=bot&permissions=8

namespace DiscordBot
{
	public class Program
	{
		static void Main(string[] args)
		{
			Bot bot = new Bot();
			bot.RunAsync().GetAwaiter().GetResult();
		}
	}
}
