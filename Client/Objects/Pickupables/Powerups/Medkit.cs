using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using Client.Models;
using SFML.Graphics;
using Client.Utilities;
using Client.Objects.Pickupables;
using Client.Objects.Pickupables.Strategy;

namespace Client.Objects
{
    class Medkit : PowerUp
    {
        public Medkit()
        {
            this.PowerUpStrategy = new HealingStrategy();
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Medkit);
        }
    }
}
