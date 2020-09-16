using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;

namespace Client.Objects.Pickupables
{
    class ReloadSyringe : Pickupable
    {
        public override void Pickup(Player player)
        {
            Console.WriteLine("Reload syringe picked up");
        }
    }
}
