using System;
using System.Collections.Generic;
using System.Text;
using Client.Objects.Destructables;
using Client.Objects.Indestructables;

namespace Client.Objects
{
    class DestructablesFactory : AbstractFactory
    {
        public override Indestructable GetIndestructable()
        {
            return null;
        }
        public override Destructable GetDestructable()
        {
            return null;
        }
    }
}
