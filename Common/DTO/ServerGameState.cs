using Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class ServerGameState
    {
        public List<ServerPlayer> Players { get; set; }
        //public List<Pickupable> Pickupables { get; set; }

        //public MapGeneration Map {get;set;}

        public ServerGameState() {
            this.Players = new List<ServerPlayer>();
        }

    }
}
