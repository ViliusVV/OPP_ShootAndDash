using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;
using Client.Config;
using Client.Utilities;
using SFML.System;

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

        public override Sprite DestroyBehavior(Vector2f position)
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Explosion);
            this.Position = position;
            return this;
        }

    }
}
