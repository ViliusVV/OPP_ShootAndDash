using System;
using System.Collections.Generic;
using System.Text;
using Client.Objects.Pickupables;

namespace Client.Objects.Abstract_Facotry.Destructibles.Bridge
{
    class MedkitBridge : IItemBridge
    {
        public Pickupable GetPickupable()
        {
            PowerupFactory pickFactory = new PowerupFactory();
            return pickFactory.GetPowerup("Medkit");
        }
    }
}
