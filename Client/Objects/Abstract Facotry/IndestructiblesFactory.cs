using System;
using System.Collections.Generic;
using System.Text;
using Client.Objects.Destructables;
using Client.Objects.Indestructables;

namespace Client.Objects
{
    class IndestructiblesFactory : AbstractFactory
    {
        public override Destructible GetDestructible(string destructibleObj)
        {
            return null;
        }
        public override Indestructible GetIndestructible(string indestructibleObj)
        {
            if (indestructibleObj == null)
                return null;

            if (indestructibleObj.Equals("Wall"))
            {
                return new Wall();
            }
            else if (indestructibleObj.Equals("BarbWire"))
            {
                return new BarbWire();
            }
            else if (indestructibleObj.Equals("Bush"))
            {
                return new Bush();
            }
            return null;
        }
    }
}
