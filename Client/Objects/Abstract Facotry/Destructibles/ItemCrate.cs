using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;
using Client.Config;
using Client.Utilities;

namespace Client.Objects.Destructables
{
    class ItemCrate : Destructible
    {
        
        public ItemCrate()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Crate);
        }

        public override Sprite SpawnObject()
        {
            return this;
        }
    }
}
