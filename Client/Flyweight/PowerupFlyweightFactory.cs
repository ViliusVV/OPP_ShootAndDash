using Client.Config;
using Client.Objects.Pickupables.Strategy;
using Client.Utilities;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Flyweight
{
    class PowerupFlyweightFactory
    {
        private static Dictionary<String, PowerupFlyweight> _powerupFlyweights = new Dictionary<String, PowerupFlyweight>();
        
        public PowerupFlyweightFactory()
        {

        }

        public static PowerupFlyweight GetFlyweight(TextureIdentifier textureIdentifier, IPowerUpStrategy strategy)
        {
            String key = getKey(textureIdentifier, strategy);
            
            if(_powerupFlyweights.ContainsKey(key))
            {
                OurLogger.Log($"Reusing flyweight {key}");

                return _powerupFlyweights[key];
            }
            else
            {
                OurLogger.Log($"Creating new flyweight {key}");

                PowerupFlyweight powerupFlyweight = new PowerupFlyweight(ResourceHolderFacade.GetInstance().Textures.Get(textureIdentifier), strategy);

                _powerupFlyweights.Add(key, powerupFlyweight);

                return _powerupFlyweights[key];
            }


        }

        public static String getKey(TextureIdentifier textureIdentifier, IPowerUpStrategy strategy)
        {
            return String.Format("{0}__{1}", textureIdentifier.ToString(), strategy.GetType().Name);
        }
    }
}
