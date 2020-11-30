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
    class MovementSyringe : PowerUp
    {
        public MovementSyringe(PowerupFlyweight powerupFlyweight) : base(powerupFlyweight) { }

        public MovementSyringe()
        {
            this.PowerUpStrategy = new MovmentSpeedStrategy();
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.MovementSyringe);
        }
    }
}
