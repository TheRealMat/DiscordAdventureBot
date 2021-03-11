using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Models
{
    public class Map
    {
        public Map()
        {
            Tiles = new HashSet<Tile>();
        }
        public virtual ICollection<Tile> Tiles { get; set; }
    }
}
