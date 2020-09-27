using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using Client.Models;
using Client.Utilities;

namespace Client.Objects.Pickupables
{
    class HealingSyringe : Pickupable
    {
        public HealingSyringe()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.HealingSyringe);
        }

        public override void Pickup(Player player)
        {
            PickedUp = true;
            Console.WriteLine("healing syringe picked up");
            player.ApplyDamage(10);
        }
    }
}
