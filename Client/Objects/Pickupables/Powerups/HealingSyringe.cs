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
    class HealingSyringe : PowerUp
    {
        public HealingSyringe(PowerupFlyweight powerupFlyweight): base(powerupFlyweight) { }

        public HealingSyringe()
        {
            this.PowerUpStrategy = new HealingStrategy();
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.HealingSyringe);
        }
    }
}
