using Client.Collisions;
using Client.Config;
using Client.Managers;
using Client.Objects;
using Client.Objects.Indestructables;
using Client.UI;
using Client.Utilities;
using Common.DTO;
using Common.Utilities;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Client.Models
{
    public class Player: Sprite, ICommand
    {
        private SoundHolder Sounds { get; set; } = SoundHolder.GetInstance();
        public string Name { get; set; } = new Random().Next(1, 888).ToString();
        public float Health { get; set; } = 100;
        public bool IsDead { get; set; } = false;

        private Vector2f _speed = new Vector2f(0.0f, 0.0f);
        public Vector2f Speed { get => _speed; set => _speed = value; }
        public Vector2f LookingAtPoint { get; set; }
        public float Heading { get; set; }

        public float SpeedMultiplier { get; set; } = 1;
        public bool Running { get => Math.Abs(Speed.X) > 0.1 || Math.Abs(Speed.Y) > 0.1; }
        public bool IsMainPlayer { get; set; } = false;
        public bool IsInvincible { get; set; } = false;
        public Weapon Weapon { get; set; }
        public Weapon[] HoldingWeapon { get; set; }
        public string PreviousWeapon { get; set; }
        public Clock SwapTimer { get; set; } = new Clock();
        public Clock DropTimer { get; set; } = new Clock();

        private PlayerAnimation PlayerAnimation { get; set; }


        public PlayerBar PlayerBar { get; set; }
        public Player()
        {
            this.PlayerBar = new PlayerBar();
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.MainCharacter);
            this.PlayerAnimation = new PlayerAnimation(this);
            this.Origin = SpriteUtils.GetSpriteCenter(this);
            this.HoldingWeapon = new Weapon[3];
        }

        public Player(ServerPlayer playerDTO)
        {
            this.PlayerBar = new PlayerBar();
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.MainCharacter);
            this.PlayerAnimation = new PlayerAnimation(this);
            this.Origin = SpriteUtils.GetSpriteCenter(this);
            this.Name = playerDTO.Name;
            this.Health = playerDTO.Health;
            this.Speed = playerDTO.Speed;
            this.Position = playerDTO.Position;
        }

        public void SetWeapon(Weapon wep)
        {
            this.PreviousWeapon = this.Weapon.Name;
            this.Weapon = wep;
        }

        public void DropWeapon()
        {
            if (this.DropTimer.ElapsedTime.AsMilliseconds() > 200)
            {
                this.DropTimer.Restart();
                if (this.Weapon != HoldingWeapon[0])
                {
                    for (int i = 1; i < HoldingWeapon.Length; i++)
                    {
                        if (HoldingWeapon[i] != null && HoldingWeapon[i].Name == this.Weapon.Name)
                        {
                            HoldingWeapon[i] = null;
                        }
                    }
                    if (HoldingWeapon[2] != null)
                    {
                        this.Weapon = HoldingWeapon[2];
                    }
                    else if (HoldingWeapon[1] != null)
                    {
                        this.Weapon = HoldingWeapon[1];
                    }
                    else
                    {
                        this.Weapon = HoldingWeapon[0];
                    }
                }
            }
        }

        //Q feature, to change weapon to last used weapon
        public void Toggle()
		{
            if (HoldingWeapon != null && this.SwapTimer.ElapsedTime.AsMilliseconds() > 200)
            {
                this.SwapTimer.Restart();
                for (int i = 0; i < HoldingWeapon.Length; i++)
                {
                    if (HoldingWeapon[i] != null && HoldingWeapon[i].Name == PreviousWeapon)
                    {
                        SetWeapon(HoldingWeapon[i]);
                        this.Weapon.Projectiles = HoldingWeapon[i].Projectiles;
                        //HoldingWeapon[i].Projectiles.Clear();
                        break;
                    }
                }
            }
		}
        public void AddHealth(float amount)
        {
            // heal player
            if(amount >= 0)
            {
                Health += amount;
                if(Health > 100)
                {
                    Health = 100;
                }
                Console.WriteLine("HP: " + Health.ToString());
            }
            // damage player
            else
            {
                Health += amount;
                if (Health <= 0)
                {
                    Health = 0;
                    IsDead = true;
                }
            }
        }

        public void TranslateFromSpeed()
        {
            var oldPos = this.Position;
            this.Position = new Vector2f(this.Position.X + Speed.X * SpeedMultiplier, this.Position.Y + Speed.Y * SpeedMultiplier);
            if (CheckCollisions())
            {
                this.Position = oldPos;
            }
            UpdatePlayerFacingPosition();
        }


        public void UpdatePlayerFacingPosition ()
        {
            if (Math.Abs(Speed.X) < 0.01)
            {
                // leave this here, it fixes facing right/left
            }
            else if (Speed.X > 0)
            {
                this.Scale = new Vector2f(1, 1);
            }
            else if (Speed.X < 0)
            {
                this.Scale = new Vector2f(-1, 1);
            }
        }


        public void UpdateSpeed()
        {
            if (IsMainPlayer && GameApplication.GetInstance().HasFocus)
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
        }

        public bool CheckCollisions()
        {
            foreach (var item in GameState.GetInstance().Collidables)
            {
                if(this.GetGlobalBounds().Intersects(item.GetGlobalBounds()))
                {
                    if(item is BarbWire)
                    {
                        this.AddHealth(-5);
                    }
                    return true;
                }
            }
            return false;
        }

        public ServerPlayer ToDTO()
        {
            var tmpDto = new ServerPlayer();

            tmpDto.Name = Name;
            tmpDto.Health = Health;
            tmpDto.Position = Position;
            tmpDto.Speed = Speed;

            return tmpDto;
        }

        public void Update()
        {
            this.PlayerAnimation.Update();
            UpdateWeapon();
            UpdatePlayerBar();

        }

        public void UpdateWeapon()
        {
            if(Weapon != null)
            {
                Weapon.Position = this.Position;

                Weapon.Rotation = this.Heading;
                Weapon.Scale = this.Heading < -90 || this.Heading > 90 ? new Vector2f(1.0f, -1.0f) : new Vector2f(1.0f, 1.0f);

                if (this.Weapon.Reloading)
                {
                    this.Weapon.ReloadGunAnimation();
                }

                if (Weapon.LaserSprite != null)
                {
                    float LaserPosition = (float)Math.Sqrt(VectorUtils.GetSquaredDistance(this.Position, this.LookingAtPoint));
                    this.Weapon.LaserSprite.Rotation = this.Heading;
                    this.Weapon.LaserSprite.Position = this.Weapon.Position;
                    this.Weapon.LaserSprite.Scale = this.Heading < -90 || this.Heading > 90 ? new Vector2f(LaserPosition / 32, 1.0f) : new Vector2f(LaserPosition / 32, -1.0f);
                }
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


        public void RefreshData(ServerPlayer playerDto)
        {
            Health = playerDto.Health;
            Position = playerDto.Position;
            Speed = playerDto.Speed;
        }
    }
}
