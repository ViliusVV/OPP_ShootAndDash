using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using Client.Models;
using Client.Utilities;

namespace Client.Objects.Pickupables
{
    class ReloadSyringe : Pickupable
    {
        public ReloadSyringe()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.ReloadSyringe);
        }
        public override void Pickup(Player player)
        {
            PickedUp = true;
            Console.WriteLine("Reload syringe picked up");
        }
    }
}
