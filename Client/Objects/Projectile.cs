using Client.Utilities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects
{
    public class Projectile : Drawable
    {
        private Vector2f _velocity = new Vector2f(0.0f, 0.0f);
        public Vector2f Velocity { get => _velocity; set => _velocity = value; }
        public float SpeedMultiplier { get; set; } = 1; 
        public float TimeSinceCreation { get; set; }
        public float DespawnBulletAfter { get; set; } = 1.5f;
        public Sprite ProjectileSprite { get; private set; }

        public Projectile(Vector2f velocity, Sprite projectileSprite, Vector2f position, float rotation)
        {
            this.Velocity = velocity;
            this.ProjectileSprite = new Sprite(projectileSprite);
            TimeSinceCreation = 0;

            ProjectileSprite.Origin = SpriteUtils.GetSpriteCenter(projectileSprite);
            ProjectileSprite.Position = position;
            ProjectileSprite.Rotation = rotation;
        }

        public void Translate()
        {
            ProjectileSprite.Position += new Vector2f(Velocity.X * SpeedMultiplier, Velocity.Y * SpeedMultiplier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime">how much time has passed between frames in seconds</param>
        public void AddDeltaTime(float deltaTime)
        {
            TimeSinceCreation += deltaTime;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(ProjectileSprite);
        }
    }
}
