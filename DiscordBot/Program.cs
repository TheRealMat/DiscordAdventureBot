using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

// Invite link: https://discordapp.com/oauth2/authorize?&client_id=817714291318849558&scope=bot&permissions=8

namespace DiscordBot
{
	public class Program
	{
		private DiscordSocketClient _client;

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient();
			_client.Log += Log;
			await _client.LoginAsync(TokenType.Bot,
				Environment.GetEnvironmentVariable("DiscordToken"));
			await _client.StartAsync();

			// Block this task until the program is closed.
			await Task.Delay(-1);
		}
		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
