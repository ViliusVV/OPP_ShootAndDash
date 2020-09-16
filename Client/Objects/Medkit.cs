using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;

namespace Client.Objects
{
    class Medkit : Pickupable
    {
        public override void Pickup(Player player)
        {
            PickedUp = true;
            Console.WriteLine("medkit picked up");
            player.Health += 10;
        }
    }
}
