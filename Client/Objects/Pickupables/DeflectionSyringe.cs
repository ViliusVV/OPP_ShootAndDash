using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;

namespace Client.Objects.Pickupables
{
    class DeflectionSyringe : Pickupable
    {
        public override void Pickup(Player player)
        {
            Console.WriteLine("Deflection syringe picked up");
        }
    }
}
