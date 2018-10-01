using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FalloutRPG.Models
{
    public class Player : BaseModel
    {
        private Player() { }

        public Player(ulong discordId)
        {
            DiscordId = discordId;
        }

        public ulong DiscordId { get; private set; }

        public Campaign Campaign { get; set; }
    }
}
