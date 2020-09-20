using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects
{
    public class Projectile : Drawable
    {
        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }

        public int TimeSinceCreation { get; set; }
        public Sprite ProjectileSprite { get; private set; }

        public Projectile(float velocityX, float velocityY, Sprite projectileSprite)
        {
            this.VelocityX = velocityX;
            this.VelocityY = velocityY;
            this.ProjectileSprite = projectileSprite;
            TimeSinceCreation = 0;
        }
        public void Move(float deltaTime)
        {
            ProjectileSprite.Position += new Vector2f(VelocityX * deltaTime, VelocityY * deltaTime);
        }
        public void InitializeSpriteParams(Vector2f origin, Vector2f position)
        {
            ProjectileSprite.Origin = origin;
            ProjectileSprite.Position = position;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(ProjectileSprite);
        }
    }
}
