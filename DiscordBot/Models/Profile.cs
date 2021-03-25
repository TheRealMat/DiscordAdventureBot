using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Models
{
    public class Profile : Entity
    {
        public ulong DiscordId { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }
    }
}
