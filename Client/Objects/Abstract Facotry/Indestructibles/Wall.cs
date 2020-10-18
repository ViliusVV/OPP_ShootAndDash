using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;
using Client.Config;
using Client.Utilities;

namespace Client.Objects.Indestructables
{
    class Wall : Indestructible
    {

        public Wall()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Wall);
        }

        public override Sprite SpawnObject()
        {
            return this;
        }
    }
}
