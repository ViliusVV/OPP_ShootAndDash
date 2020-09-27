using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using Client.Models;
using Client.Utilities;

namespace Client.Objects.Pickupables
{
    class DeflectionSyringe : Pickupable
    {
        public DeflectionSyringe()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.DeflectionSyringe);
        }

        public override void Pickup(Player player)
        {
            PickedUp = true;
            Console.WriteLine("Deflection syringe picked up");
        }
    }
}
