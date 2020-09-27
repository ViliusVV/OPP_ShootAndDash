using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using Client.Models;
using Client.Utilities;

namespace Client.Objects.Pickupables
{
    class MovementSyringe : Pickupable
    {
        public MovementSyringe()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.MovementSyringe);
        }

        public override void Pickup(Player player)
        {
            PickedUp = true;
            player.IncreaseMovementSpeed(2, 3000);
            Console.WriteLine("Movement syringe picked up");
        }
    }
}
