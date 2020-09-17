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
            PickedUp = true;
            player.IncreaseMovementSpeed(2, 3000);
            Console.WriteLine("Movement syringe picked up");
        }
    }
}
