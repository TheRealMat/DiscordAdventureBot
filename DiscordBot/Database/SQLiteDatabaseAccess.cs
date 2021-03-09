using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Text;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Database
{
	public class SQLiteDatabaseAccess
	{

		public static List<Entity> LoadEntities()
		{
			using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = conn.Query<Entity>("select * from Entity", new DynamicParameters());
				return output.ToList();
			}

		}

		public static void SaveEntity(Entity entity)
		{
			using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
			{
				conn.Execute("insert into Entity (discordID, name, age) values (@DiscordID, @Name, @Age)", entity);
			}
		}

		public static void RemoveMe(string discordID)
		{
			using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
			{
				// This is safe, No sql injection. The ID comes from the context
				conn.Execute($"DELETE FROM Entity WHERE discordID = {discordID}");
			}
		}

		private static string LoadConnectionString(string id = "Default")
		{
			return ConfigurationManager.ConnectionStrings[id].ConnectionString;
		}

	}
}
