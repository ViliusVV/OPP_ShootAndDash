using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using Client.Models;
using SFML.Graphics;
using Client.Utilities;
using Client.Objects.Pickupables;
using Client.Objects.Pickupables.Strategy;
using Client.Flyweight;

namespace Client.Objects
{
    public class Medkit : PowerUp
    {
        public Medkit(PowerupFlyweight powerupFlyweight) : base(powerupFlyweight) { }

        public Medkit()
        {
            this.PowerUpStrategy = new HealingStrategy();
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Medkit);
        }
    }
}
