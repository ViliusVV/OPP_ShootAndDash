using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using Client.Models;
using SFML.Graphics;

namespace Client.Objects
{
    public class Medkit : Pickupable
    {
        public Medkit()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Medkit);
        }

        public override void Pickup(Player player)
        {
            PickedUp = true;
            Console.WriteLine("medkit picked up");
            player.ApplyDamage(50);
        }
    }
}
