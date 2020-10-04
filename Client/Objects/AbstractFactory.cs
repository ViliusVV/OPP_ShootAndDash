using System;
using System.Collections.Generic;
using System.Text;
using Client.Objects.Destructables;
using Client.Objects.Indestructables;

namespace Client.Objects
{
    abstract class AbstractFactory : GameApplication
    {
        public abstract Destructible GetDestructible(string destructibleObj);
        public abstract Indestructible GetIndestructible(string IndestructibleObj);
    }
}
