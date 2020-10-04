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
        private Sprite wallObject;

        public Wall()
        {
            wallObject = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Wall));
        }

        public override Sprite SpawnObject()
        {
            return wallObject;
        }
    }
}
