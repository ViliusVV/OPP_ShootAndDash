using Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class GameState
    {
        public List<PlayerDTO> Players { get; set; }
        //public List<Pickupable> Pickupables { get; set; }

        //public MapGeneration Map {get;set;}

        public GameState() {
            this.Players = new List<PlayerDTO>();
        }

    }
}
