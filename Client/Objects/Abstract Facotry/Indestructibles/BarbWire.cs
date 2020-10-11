using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using Client.Config;
using Client.Utilities;

namespace Client.Objects.Indestructables
{
    class BarbWire : Indestructible
    {
        private Sprite barbWireObject;

        public BarbWire()
        {
            barbWireObject = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.BarbWire));
        }

        public override Sprite SpawnObject()
        {
            return barbWireObject;
        }
    }
}
