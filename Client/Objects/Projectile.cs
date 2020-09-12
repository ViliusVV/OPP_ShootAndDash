using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects
{
    class Projectile
    {
        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }

        //Texture ProjectileTexture ;
        public Sprite ProjectileSprite { get; private set; }

        public Projectile(float velocityX, float velocityY, Sprite projectileSprite)
        {
            this.VelocityX = velocityX;
            this.VelocityY = velocityY;
            this.ProjectileSprite = projectileSprite;
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
    }
}
