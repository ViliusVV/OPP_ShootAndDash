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
        public static float Damage { get; set; } = 1f;
        
        public BarbWire()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.BarbWire);
        }

        public override Sprite SpawnObject()
        {
            return this;
        }
    }
}
