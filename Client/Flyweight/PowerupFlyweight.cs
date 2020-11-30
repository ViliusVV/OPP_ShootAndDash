using Client.Objects.Pickupables.Strategy;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Flyweight
{
    public class PowerupFlyweight
    {
        public Texture Texture { get; private set; }
        public IPowerUpStrategy Strategy { get; private set; }

        public PowerupFlyweight(Texture texture, IPowerUpStrategy strategy)
        {
            this.Texture = texture;
            this.Strategy = strategy;
        }

        public override bool Equals(object obj)
        {
            return obj is PowerupFlyweight flyweight &&
                   EqualityComparer<Texture>.Default.Equals(Texture, flyweight.Texture) &&
                   EqualityComparer<IPowerUpStrategy>.Default.Equals(Strategy, flyweight.Strategy);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Texture, Strategy);
        }
    }
}
