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
                return new DeflectionSyringe();
            }
            else if (powerupType.Equals("HealingSyringe"))
            {
                return new HealingSyringe();
            }
            else if (powerupType.Equals("Medkit"))
            {
                var tmpMedkit = new Medkit();
                tmpMedkit.PowerUpStrategy = new HealingStrategy(100f);

                return tmpMedkit;
            }
            else if (powerupType.Equals("MovementSyringe"))
            {
                return new MovementSyringe();
            }
            else if (powerupType.Equals("ReloadSyringe"))
            {
                return new ReloadSyringe();
            }
            return null;
        }
    }
}
