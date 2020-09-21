using Client.Collisions;
using Client.Objects;
using Client.Utilities;
using Common.DTO;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    class Player: Sprite
    {
        public string Name { get; set; } = new Random().Next(1, 888).ToString();
        public float Health { get; private set; } = 100;

        public Vector2f Speed { get; set; } = new Vector2f(0.0f, 0.0f);

        public bool IsDead { get; private set; } = false;
        public float SpeedMultiplier { get; private set; } = 1;

        public Weapon Weapon { get; private set; }
        public Player() { }
        public Player(PlayerDTO playerDTO)
        {
            Name = playerDTO.Name;
            Health = playerDTO.Health;
            Speed = playerDTO.Speed;
            Position = playerDTO.Position;
            TextureRect = new IntRect(0, 0, 36, 64);
        }
        public void SetWeapon(Weapon wep)
        {
            this.Weapon = wep;
        }
        public void ApplyDamage(float amount)
        {
            float newHealth = Health + amount;
            if (newHealth <= 100)
            {
                Health += amount;
                if (Health <= 0)
                {
                    IsDead = true;
                    Health = 0;
                }
            }
            else
            {
                Health = 100;
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
        public float GetHealth(float scale)
        {
            if (Health > 0)
            {
                return scale * Health / 100;
            }
            else
            {
                return 0;
            }
        }

        public float HealthOffSet(float scale)
        {
            if (Health > 0)
            { 
            return (float)15.5*((100 - Health) * scale / 100) ;
            }
            else
            {
                return 0;
            }
        }

        public PlayerDTO ToDTO()
        {
            var tmpDto = new PlayerDTO();

            tmpDto.Name = Name;
            tmpDto.Health = Health;
            tmpDto.Position = Position;
            tmpDto.Speed = Speed;

            return tmpDto;
        }


        public void RefreshData(PlayerDTO playerDto)
        {
            Health = playerDto.Health;
            Position = playerDto.Position;
            Speed = playerDto.Speed;
        }

    }
}
