using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;

namespace Client.Objects.Pickupables
{
    class HealingSyringe : Pickupable
    {
        public override void Pickup(Player player)
        {
            Console.WriteLine("healing syringe picked up");
        }
    }
}
