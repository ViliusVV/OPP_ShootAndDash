using Client.Models;
using Client.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers
{
    class GameState
    {
        public List<Player> Players { get; set; }
        public List<Pickupable> Pickupables { get; set; }

        public GameState()
        {
            this.Players = new List<Player>();
            this.Pickupables = new List<Pickupable>();
        }
    }
}
