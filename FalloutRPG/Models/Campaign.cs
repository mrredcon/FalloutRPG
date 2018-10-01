using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutRPG.Models
{
    public class Campaign : BaseModel
    {
        private Campaign() { }

        public Campaign(string name, ulong owner, ulong roleId, ulong textChannelId)
        {
            Name = name;
            OwnerId = owner;
            RoleId = roleId;
            TextChannelId = textChannelId;
        }

        public string Name { get; private set; }

        public ulong OwnerId { get; private set; }
        public ulong RoleId { get; private set; }
        public ulong TextChannelId { get; private set; }
    }
}
