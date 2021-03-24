using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DiscordBot.Commands;
using System;

namespace DiscordBot
{
	class Bot
	{
		public DiscordClient client { get; private set; }
		public CommandsNextExtension Commands { get; private set; }

		public Bot(IServiceProvider services)
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
				//IgnoreExtraArguments = false,
				EnableMentionPrefix = true,
				Services = services
			};

			Commands = client.UseCommandsNext(commandsConfig);
			Commands.RegisterCommands<TestCommands>();
			Commands.RegisterCommands<RPGCommands>();
			Commands.RegisterCommands<ProfileCommands>();

			client.ConnectAsync();
		}


		private Task OnClientReady(object sender, ReadyEventArgs e)
		{
			return Task.CompletedTask;
		}
	}
}
