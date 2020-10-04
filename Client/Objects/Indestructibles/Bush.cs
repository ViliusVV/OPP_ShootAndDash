using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;
using Client.Config;
using Client.Utilities;

namespace Client.Objects.Indestructables
{
    class Bush : Indestructible
    {
        private Sprite bushObject;

        public Bush()
        {
            bushObject = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Bush));
        }

        public override Sprite SpawnObject()
        {
            return bushObject;
        }
    }
}
