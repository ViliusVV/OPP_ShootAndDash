using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Util
{
    static class Util
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
}
