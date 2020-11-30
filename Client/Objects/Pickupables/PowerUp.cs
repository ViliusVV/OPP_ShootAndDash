using Client.Flyweight;
using Client.Models;
using Client.Objects.Pickupables.Strategy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Pickupables
{
    public abstract class PowerUp : Pickupable
    {
        public IPowerUpStrategy PowerUpStrategy { get; set; }

        public PowerUp()
        {

        }

        public PowerUp(PowerupFlyweight powerupFlyweight)
        {
            this.Texture = powerupFlyweight.Texture;
            this.PowerUpStrategy = powerupFlyweight.Strategy;
        }

        public override void Pickup(Player player)
        {
            this.PickedUp = true;

            this.PowerUpStrategy.DoPowerUpLogic(player);
        }
    }
}
