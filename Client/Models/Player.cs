using Client.Collisions;
using Client.Config;
using Client.Managers;
using Client.Objects;
using Client.UI;
using Client.Utilities;
using Common.DTO;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    class Player: Sprite
    {
        public string Name { get; set; } = new Random().Next(1, 888).ToString();
        public float Health { get; private set; } = 100;

        private Vector2f _speed = new Vector2f(0.0f, 0.0f);
        public Vector2f Speed { get => _speed; set => _speed = value; }

        public bool IsDead { get; private set; } = false;
        public float SpeedMultiplier { get; private set; } = 1;

        public Weapon Weapon { get; set; }

        public PlayerBar PlayerBar { get; set; }
        public Player()
        {
            this.PlayerBar = new PlayerBar();
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.MainCharacter);
        }


        public Player(PlayerDTO playerDTO)
        {
            this.PlayerBar = new PlayerBar();
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.MainCharacter);
            this.TextureRect = new IntRect(0, 0, 36, 64);
            this.Origin = SpriteUtils.GetSpriteCenter(this);
            this.Name = playerDTO.Name;
            this.Health = playerDTO.Health;
            this.Speed = playerDTO.Speed;
            this.Position = playerDTO.Position;
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
        public void TranslateFromSpeed()
        {
            var oldPos = this.Position;
            this.Position = new Vector2f(this.Position.X + Speed.X * SpeedMultiplier, this.Position.Y + Speed.Y * SpeedMultiplier);
            if (CheckCollisions())
            {
                this.Position = oldPos;
            }
        }
        public void UpdateSpeed()
        {
            bool left = Keyboard.IsKeyPressed(Keyboard.Key.A);
            bool right = Keyboard.IsKeyPressed(Keyboard.Key.D); 
            bool up = Keyboard.IsKeyPressed(Keyboard.Key.W); 
            bool down = Keyboard.IsKeyPressed(Keyboard.Key.S);
            const float increment = 0.4f;
            const float maxSpd = 4;
            float xIncrement = 0;
            float yIncrement = 0;

            // process movement
            xIncrement -= left ? increment : -increment;
            xIncrement += right ? increment : -increment;
            yIncrement -= up ? increment : -increment;
            yIncrement += down ? increment : -increment;

            // normalize movement distance when moving diagonally 
            if (xIncrement != 0 && yIncrement != 0)
            {
                xIncrement /= 1.41f;
                yIncrement /= 1.41f;
            }

            // slow down if player isnt moving
            if ((Math.Abs(xIncrement) < 0.1f) && Speed.X > 0)
                xIncrement -= increment;
            if ((Math.Abs(xIncrement) < 0.1f) && Speed.X < 0)
                xIncrement += increment;
            if ((Math.Abs(yIncrement) < 0.1f) && Speed.Y > 0)
                yIncrement -= increment;
            if ((Math.Abs(yIncrement) < 0.1f) && Speed.Y < 0)
                yIncrement += increment;

            _speed.X += xIncrement;
            _speed.Y += yIncrement;

            // limit speed
            if (Speed.X > maxSpd)
                _speed.X = maxSpd;
            if (Speed.X < -maxSpd)
                _speed.X = -maxSpd;
            if (Speed.Y > maxSpd)
                _speed.Y = maxSpd;
            if (Speed.Y < -maxSpd)
                _speed.Y = -maxSpd;

            // if speed is close to 0, set it to 0
            if (Math.Abs(Speed.X) < 0.3f)
                _speed.X = 0;
            if (Math.Abs(Speed.Y) < 0.3f)
                _speed.Y = 0;
        }


        public bool CheckMovementCollision(float xOffset, float yOffset, Sprite targetCollider)
        {
            Translate(xOffset, yOffset);
            if (CollisionTester.BoundingBoxTest(this, targetCollider))
            {
                Translate(-xOffset, -yOffset);
                Speed = new Vector2f(0, 0);
                return true;
            }
            else
            {
                Translate(-xOffset, -yOffset);
                return false;
            }
        }

        public bool CheckCollisions()
        {
            foreach (var item in GameState.GetInstance().Collidables)
            {
                if(CollisionTester.BoundingBoxTest(this, item))
                {
                    return true;
                }
            }
            return false;
        }

        public void IncreaseMovementSpeed(float multiplier, float durationInMilis)
        {
            SpeedMultiplier = 2;
            Task.Delay((int)durationInMilis).ContinueWith(o => SpeedMultiplier = 1);
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

        public void Update()
        {
            UpdatePlayerBar();
            UpdateWeapon();

        }

        public void UpdateWeapon()
        {
            if(Weapon != null)
            {
                Weapon.Position = this.Position;
            }
        }

        public void UpdatePlayerBar()
        {
            float ammo = 0.0f;
            if(Weapon != null)
            {
                ammo = (float)Weapon.Ammo / Weapon.MagazineSize * 100;
            }

            PlayerBar.Update(Health , ammo);
        }


        public void RefreshData(PlayerDTO playerDto)
        {
            Health = playerDto.Health;
            Position = playerDto.Position;
            Speed = playerDto.Speed;
        }

    }
}
