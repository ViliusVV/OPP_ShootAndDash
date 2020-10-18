using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;
using Client.Config;
using Client.Utilities;

namespace Client.Objects.Destructables
{
    class ExplosiveBarrel : Destructible
    {

        public ExplosiveBarrel()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.ExplosiveBarrel);
        }

        public override Sprite SpawnObject()
        {
            return this;
        }
    }
}
