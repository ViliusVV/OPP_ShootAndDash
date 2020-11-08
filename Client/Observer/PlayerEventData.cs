using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Observer
{
    class PlayerEventData
    {
        public Player Shooter { get; set; }
        public Player Victim { get; set; }
    }
}
