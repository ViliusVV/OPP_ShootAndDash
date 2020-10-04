using System;
using System.Collections.Generic;
using System.Text;
using Client.Objects.Destructables;
using Client.Objects.Indestructables;

namespace Client.Objects
{
    class IndestructablesFactory : AbstractFactory
    {
        public override Destructable GetDestructable()
        {
            return null;
        }
        public override Indestructable GetIndestructable()
        {
            return null;
        }
    }
}
