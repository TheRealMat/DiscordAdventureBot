using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
namespace DiscordBot
{
	class Bot
	{
		public DiscordClient client { get; private set; }
		public CommandsNextExtension Commands { get; private set; }

		public async Task RunAsync()
		{
			var config = Config.Instance;

			client = new DiscordClient(new DiscordConfiguration
			{
				Token = config.getConfig().Token,
				TokenType = TokenType.Bot,
				AutoReconnect = true,
				MinimumLogLevel = LogLevel.Debug
			});

			client.Ready += OnClientReady;

			var commandsConfig = new CommandsNextConfiguration
			{
				StringPrefixes = new string[] { config.getConfig().Prefix },
				EnableDms = false,
				EnableMentionPrefix = true
			};

			Commands = client.UseCommandsNext(commandsConfig);

			await client.ConnectAsync();

			await Task.Delay(-1);
		}

		private Task OnClientReady(object sender, ReadyEventArgs e)
		{
			return Task.CompletedTask;
		}
	}
}
