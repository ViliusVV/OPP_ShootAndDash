using Client.Models;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Template
{
    public abstract class TrapSpawner : Pickupable
    {
        public abstract Sprite ApplySkin();
        public abstract void ApplyDamage(Player player);
        public abstract void ApplyBehavior(Player player);

        public void BuildTrap()
        {
            ApplySkin(); // think of something else? 
        }

        public override void Pickup(Player player)
        {
            this.PickedUp = true;

            ApplyDamage(player);
            ApplyBehavior(player);
        }
    }
}
