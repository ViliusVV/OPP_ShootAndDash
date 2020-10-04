using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Client.Objects.Indestructables
{
    class BarbWire : Indestructable
    {
        private Sprite barbWireObject;

        public BarbWire()
        {

        }

        public override Sprite SpawnObject()
        {
            return barbWireObject;
        }
    }
}
