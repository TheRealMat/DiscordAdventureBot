using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiscordBot
{
	struct ConfigJson
	{
		[JsonProperty("token")]
		public string Token { get; private set; }

		[JsonProperty("prefix")]
		public string Prefix { get; private set; }
	}

	class Config
	{

		private static Config instance = null;
		public static Config Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Config();
				}
				return instance;
			}
		}

		private ConfigJson configJson;

		private Config()
		{
			var json = string.Empty;
			using (var fs = File.OpenRead("config.json"))
			{
				using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
				{
					json = sr.ReadToEnd();
				}
			}

			configJson = JsonConvert.DeserializeObject<ConfigJson>(json);
		}


		public ConfigJson getConfig()
		{
			return configJson;
		}
	}

}
