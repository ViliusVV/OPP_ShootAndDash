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
            PickedUp = true;
            Console.WriteLine("healing syringe picked up");
            player.ApplyDamage(-10);
        }
    }
}
