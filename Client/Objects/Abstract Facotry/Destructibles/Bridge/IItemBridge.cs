using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Abstract_Facotry.Destructibles.Bridge
{
    interface IItemBridge
    {
        public Pickupable GetPickupable();
    }
}
