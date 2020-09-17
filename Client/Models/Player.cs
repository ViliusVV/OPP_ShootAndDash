using Client.Collisions;
using Client.Utilities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    class Player: Sprite
    {
        public float Health { get; private set; } = 100;

        public Vector2f Speed { get; set; } = new Vector2f(0.0f, 0.0f);

        public bool IsDead { get; private set; } = false;
        public float SpeedMultiplier { get; private set; } = 1;
        //public Position Position { get; set; } = new Position();

        public void ApplyDamage(float amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                IsDead = true;
                this.Scale = new Vector2f(0.1f, 0.1f);
            }
        }
        public void Translate(float xOffset, float yOffset)
        {
            this.Position = new Vector2f(this.Position.X + xOffset * SpeedMultiplier, this.Position.Y + yOffset * SpeedMultiplier);
        }
        public bool CheckMovementCollision(float xOffset, float yOffset, Sprite targetCollider)
        {
            Translate(xOffset, yOffset);
            if (CollisionTester.BoundingBoxTest(this, targetCollider))
            {
                Translate(-xOffset, -yOffset);
                return true;
            }
            else
            {
                Translate(-xOffset, -yOffset);
                return false;
            }
        }
        public void IncreaseMovementSpeed(float multiplier, float durationInMilis)
        {
            SpeedMultiplier = 2;
            Task.Delay((int)durationInMilis).ContinueWith(o => SpeedMultiplier = 1);
        }
        //// Configure text
        //Font font = Fonts.Get(FontIdentifier.PixelatedSmall);
        //Text text = new Text("000 000", font)
        //{
        //    CharacterSize = 14,
        //    OutlineThickness = 2.0f

        //};
        //float textWidth = text.GetLocalBounds().Width;
        //float textHeight = text.GetLocalBounds().Height;
        //float xOffset = text.GetLocalBounds().Left + 30;
        //float yOffset = text.GetLocalBounds().Top + 30;
        //text.Origin = new Vector2f(textWidth / 2f + xOffset, textHeight / 2f + yOffset);
        //text.Position = new Vector2f(position.X, position.Y);
    }
}
