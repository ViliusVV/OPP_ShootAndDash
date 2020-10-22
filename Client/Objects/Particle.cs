using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using Client.Utilities;
using SFML.Graphics;
using SFML.System;

namespace Client.Objects
{
    class Particle : Sprite
    {
        public Particle(Vector2f position)
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Explosion);
            this.Position = position;
        }
    }
}
