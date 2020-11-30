using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using Client.Flyweight;
using Client.Models;
using Client.Objects.Pickupables.Strategy;
using Client.Utilities;

namespace Client.Objects.Pickupables
{
    class DeflectionSyringe : PowerUp
    {
        public DeflectionSyringe(PowerupFlyweight flyweight) : base(flyweight) { }

        public DeflectionSyringe()
        {
            this.PowerUpStrategy = new DeflectionStrategy();
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.DeflectionSyringe);
        }
    }
}
