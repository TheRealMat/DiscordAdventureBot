using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Models
{
    public class TileEntity : Entity
    {
        public string Graphic { get; set; }
        public Tile CurrentTile { get; set; }
    }
}
