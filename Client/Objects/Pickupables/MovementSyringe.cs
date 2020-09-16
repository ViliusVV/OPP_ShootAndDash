using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;

namespace Client.Objects.Pickupables
{
    class MovementSyringe : Pickupable
    {
        public override void Pickup(Player player)
        {
            Console.WriteLine("Movement syringe picked up");
        }
    }
}
