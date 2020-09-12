using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

using Client.Utilities;

namespace Client.UI
{
    class AimCursor : Drawable
    {
        private Sprite cursorSprite { get; set; }
        public Vector2f position { get; set; }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(cursorSprite);
        }

        public void Update(Vector2f pos)
        {
            this.position = pos;
            cursorSprite.Position = pos;
        }

        public void SetSprite(Sprite sprite)
        {
            sprite.Origin = SpriteUtils.GetSpriteCenter(sprite);
            sprite.Scale = new Vector2f(2.0f, 2.0f);
            this.cursorSprite = sprite;
        }

        public void SetTexture(Texture texture)
        {
            this.SetSprite(new Sprite(texture));
        }

    }
}
