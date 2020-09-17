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
            PickedUp = true;
            Console.WriteLine("Reload syringe picked up");
        }
    }
}
