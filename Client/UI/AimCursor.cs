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
        private Sprite CursorSprite { get; set; }
        public Vector2f Position { get; set; }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(CursorSprite);
        }

        public void Update(Vector2f pos)
        {
            this.Position = pos;
            CursorSprite.Position = pos;
        }

        public void SetSprite(Sprite sprite)
        {
            sprite.Origin = SpriteUtils.GetSpriteCenter(sprite);
            sprite.Scale = new Vector2f(2.0f, 2.0f);
            this.CursorSprite = sprite;
        }

        public void ChangeSize(float size)
		{
            this.CursorSprite.Scale = new Vector2f(size, size);
		}

        public void SetTexture(Texture texture)
        {
            this.SetSprite(new Sprite(texture));
        }

    }
}
