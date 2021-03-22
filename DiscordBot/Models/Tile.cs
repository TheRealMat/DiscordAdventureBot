using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Models
{
    public class Tile : Entity
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public string graphic { get; set; }
    }
}
