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
using Common.Enums;
using Client.Adapters;

namespace Client.Models
{
    public class Player: Sprite, ICommand
    {
        private SoundHolder Sounds { get; set; } = SoundHolder.GetInstance();
        public string Name { get; set; }
        public float Health { get; set; } = 100;
        public bool IsDead { get => Health <= 0; }

        public int Kills { get; set; }
        public int Deaths { get; set; }

        private Vector2f _speed = new Vector2f(0.0f, 0.0f);
        public Vector2f Speed { get => _speed; set => _speed = value; }
        public Vector2f LookingAtPoint { get; set; }
        public float Heading { get; set; }

        public virtual float SpeedMultiplier { get; set; } = 1;
        public bool Running { get => Math.Abs(Speed.X) > 0.1 || Math.Abs(Speed.Y) > 0.1; }
        public bool IsMainPlayer { get; set; } = false;
        public bool IsInvincible { get; set; } = false;
        public Weapon Weapon { get; set; }
        public Weapon[] HoldingWeapon { get; set; }
        public string PreviousWeapon { get; set; }
        public Clock SwapTimer { get; set; } = new Clock();
        public Clock DropTimer { get; set; } = new Clock();

        private PlayerAnimation PlayerAnimation { get; set; }
        private PlayerSkinType Appearance { get; set; } = Utils.RandomEnum<PlayerSkinType>();


        public PlayerBar PlayerBar { get; set; }


        public Player()
        {
            InitPlayer(this.Appearance);
        }

        public Player(PlayerSkinType skinType)
        {
            InitPlayer(skinType);
        }

        public Player(ServerPlayer playerDTO)
        {
            InitPlayer(playerDTO.Appearance);

            this.Name = playerDTO.Name;
            this.Health = playerDTO.Health;
            this.Speed = playerDTO.Speed;
            this.Position = playerDTO.Position;
        }

        public void InitPlayer(PlayerSkinType appearance)
        {
            this.Name = String.Format("Player-{0}", new Random().Next(111, 999).ToString());
            this.Appearance = appearance;
            this.Texture = GetTexture(this.Appearance);
            this.PlayerAnimation = new PlayerAnimation(this);
            this.Origin = SpriteUtils.GetSpriteCenter(this);

            this.HoldingWeapon = new Weapon[3];

            this.PlayerBar = new PlayerBar();
        }

        public void SetWeapon(Weapon wep)
        {
            this.PreviousWeapon = this.Weapon.Name;
            this.Weapon = wep;
        }

        private Texture GetTexture(PlayerSkinType skinType)
        {
            TextureHolder textures = ResourceHolderFacade.GetInstance().Textures;

            return skinType switch
            {
                PlayerSkinType.CheerfulAssasin => textures.Get(TextureIdentifier.CheerfulAssasinChar),
                PlayerSkinType.TriggerHappyHipster => textures.Get(TextureIdentifier.TriggerHappyHipsterChar),
                PlayerSkinType.HawaiianManiac => textures.Get(TextureIdentifier.HawaiianManiacChar),
                PlayerSkinType.ProfoundAsian => textures.Get(TextureIdentifier.ProfoundAsianChar),
                /* default */ _ => textures.Get(TextureIdentifier.CheerfulAssasinChar),
            };
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
                            this.Weapon.mediator.Send("dropped", this.Weapon);
                            PlayerBar.TimeOutTimer.Restart();
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
            }
            // damage player
            else
            {
                Health += amount;
                if (Health <= 0)
                {
                    Health = 0;
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
                bool left = false;
                bool right = false;
                bool up = false;
                bool down = false;
                if (GameState.GetInstance().ControlsCheck)
                {
                    left = Keyboard.IsKeyPressed(Keyboard.Key.A);
                    right = Keyboard.IsKeyPressed(Keyboard.Key.D);
                    up = Keyboard.IsKeyPressed(Keyboard.Key.W);
                    down = Keyboard.IsKeyPressed(Keyboard.Key.S);
                }
                else
				{
                    left = Keyboard.IsKeyPressed(Keyboard.Key.Left);
                    right = Keyboard.IsKeyPressed(Keyboard.Key.Right);
                    up = Keyboard.IsKeyPressed(Keyboard.Key.Up);
                    down = Keyboard.IsKeyPressed(Keyboard.Key.Down);
                }
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


        public ServerPlayer ToDTO()
        {
            var tmpDto = new ServerPlayer
            {
                Name = Name,
                Health = Health,
                Position = Position,
                Speed = Speed,
                Heading = Heading,
                IsDead = IsDead,
                Appearance = Appearance,
                Kills = Kills,
                Deaths = Deaths,
                ServerWeapon = new ServerWeaponAdapter(Weapon)
            };

            return tmpDto;
        }

        public void RefreshData(ServerPlayer playerDto)
        {
            this.Health = playerDto.Health;
            this.Position = playerDto.Position;
            this.Speed = playerDto.Speed;
            this.Heading = playerDto.Heading;

            this.Kills = Kills;
            this.Deaths = Deaths;


            if(this.Weapon == null)
            {
                this.Weapon = Weapon.CreateWeapon(playerDto.ServerWeapon.WeaponType);
                this.HoldingWeapon[0] = this.Weapon;
            }
            else if(ServerWeaponAdapter.GetType(Weapon) != playerDto.ServerWeapon.WeaponType)
            {
                Weapon newWeapon = Weapon.CreateWeapon(playerDto.ServerWeapon.WeaponType);
                newWeapon.Projectiles.AddRange(this.Weapon.Projectiles); // not pretty, but good enough

                this.Weapon = newWeapon;
                this.HoldingWeapon[0] = this.Weapon;
            }

            this.Weapon.Ammo = playerDto.ServerWeapon.Ammo;


        }

        public override bool Equals(object obj)
        {
            return obj is Player player &&
                   Name == player.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
