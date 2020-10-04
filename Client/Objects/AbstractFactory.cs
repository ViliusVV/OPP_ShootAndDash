using System;
using System.Collections.Generic;
using System.Text;
using Client.Objects.Destructables;
using Client.Objects.Indestructables;

namespace Client.Objects
{
    abstract class AbstractFactory : GameApplication
    {
        public abstract Destructable GetDestructable();
        public abstract Indestructable GetIndestructable();
    }
}
