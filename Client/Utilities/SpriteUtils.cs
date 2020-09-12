using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Utilities
{
    static class SpriteUtils
    {
        public static Vector2f GetSpriteCenter(Sprite sprite)
        {
            Vector2f size = GetSpriteSize(sprite);
            return new Vector2f(size.X / 2.0f, size.Y / 2.0f);
        }

        public static Vector2f GetSpriteSize(Sprite sprite)
        {
            float xSize = sprite.Scale.X * sprite.Texture.Size.X;
            float ySize = sprite.Scale.Y * sprite.Texture.Size.Y;
            

            return new Vector2f(xSize, ySize);
        }
    }

    class Position
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Position() { }

        public Position(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Vector2f toVec2f()
        {
            return new Vector2f(this.X, this.Y);
        }
    }
}
