using System;
using System.Collections.Generic;
using System.Text;
using Client.Objects.Destructables;
using Client.Objects.Indestructables;

namespace Client.Objects
{
    class DestructiblesFactory : AbstractFactory
    {
        public override Indestructible GetIndestructible(string indestructibleObj)
        {
            return null;
        }
        public override Destructible GetDestructible(string destructibleObj)
        {
            if (destructibleObj.Equals("LandMine"))
            {
                return new LandMine();
            }
            else if (destructibleObj.Equals("ItemCrate"))
            {
                return new ItemCrate();
            }
            return null;
        }
    }
}
