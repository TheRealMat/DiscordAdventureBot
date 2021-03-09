using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiscordBot.Database
{
	public class Entity
	{
		[Key]
		public int Id { get; set; }

		public string DiscordID { get; set; }

		public string Name { get; set; }

		public int Age { get; set; }
	}
}
