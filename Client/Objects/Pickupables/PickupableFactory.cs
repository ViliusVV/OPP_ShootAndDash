using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Pickupables
{
    class PickupableFactory
    {
        public Pickupable GetPickupable(string pickupableType)
        {
            if (pickupableType == null)
                return null;

            if (pickupableType.Equals("DeflectionSyringe"))
            {
                return new DeflectionSyringe();
            }
            else if (pickupableType.Equals("HealingSyringe"))
            {
                return new HealingSyringe();
            }
            else if (pickupableType.Equals("Medkit"))
            {
                return new Medkit();
            }
            else if (pickupableType.Equals("MovementSyringe"))
            {
                return new MovementSyringe();
            }
            else if (pickupableType.Equals("ReloadSyringe"))
            {
                return new ReloadSyringe();
            }
            return null;
        }
    }
}
