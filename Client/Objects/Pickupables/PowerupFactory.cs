using Client.Config;
using Client.Flyweight;
using Client.Objects.Pickupables.Strategy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Pickupables
{
    class PowerupFactory
    {
        public PowerUp GetPowerup(string powerupType)
        {

            if (powerupType == null)
                return null;

            if (powerupType.Equals("DeflectionSyringe"))
            {
                PowerupFlyweight flyweight = PowerupFlyweightFactory.GetFlyweight(TextureIdentifier.DeflectionSyringe, new DeflectionStrategy());
                return new DeflectionSyringe(flyweight);
            }
            else if (powerupType.Equals("HealingSyringe"))
            {
                PowerupFlyweight flyweight = PowerupFlyweightFactory.GetFlyweight(TextureIdentifier.HealingSyringe, new HealingStrategy());
                return new HealingSyringe(flyweight);
            }
            else if (powerupType.Equals("Medkit"))
            {
                PowerupFlyweight flyweight = PowerupFlyweightFactory.GetFlyweight(TextureIdentifier.Medkit, new HealingStrategy());
                var tmpMedkit = new Medkit(flyweight);

                tmpMedkit.PowerUpStrategy = new HealingStrategy(100f);

                return tmpMedkit;
            }
            else if (powerupType.Equals("MovementSyringe"))
            {
                PowerupFlyweight flyweight = PowerupFlyweightFactory.GetFlyweight(TextureIdentifier.MovementSyringe, new MovmentSpeedStrategy());
                return new MovementSyringe(flyweight);
            }
            else if (powerupType.Equals("ReloadSyringe"))
            {
                PowerupFlyweight flyweight = PowerupFlyweightFactory.GetFlyweight(TextureIdentifier.ReloadSyringe, new ReloadSpeedStrategy());
                return new ReloadSyringe(flyweight);
            }
            return null;
        }
    }
}
