using Client.Collisions;
using Client.Config;
using Client.Managers;
using Client.Models;
using Client.Objects.Abstract_Facotry.Destructibles.Bridge;
using Client.Objects.Destructables;
using Client.Objects.Pickupables;
using Client.Objects.Pickupables.Decorator;
using Client.Objects.Prototype;
using Client.UI;
using Client.Utilities;
using Common.DTO;
using Common.Enums;
using Common.Utilities;
using Microsoft.AspNetCore.SignalR.Client;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Client.Objects
{
    public class Weapon : Pickupable, IWeaponPrototype
    {
        public string Name { get; set; }
        public int MagazineSize { get; set; }
        public int Ammo { get; set; }
        public float Damage { get; set; }
        public float ProjectileSpeed { get; set; }
        public float AttackSpeed { get; set; }
        public float ReloadDuration { get; set; }
        public int SpreadAmount { get; set; }
        public bool CanShoot { get; set; }
        public bool Reloading { get; set; }
        public string LaserSight { get; set; }
        public List<Projectile> Projectiles { get; set;}
        public Clock ShootTimer { get; set; } = new Clock();
        public Clock ReloadTimer { get; set; } = new Clock();
        public Clock ReloadCooldown { get; set; } = new Clock();
        public Sprite ProjectileSprite { get; set; }
        public Sprite LaserSprite { get; set; }
        

        public Weapon()
        {
            //this.Name = name;
            //this.MagazineSize = magazineSize;
            //this.Ammo = magazineSize;
            //this.Damage = dmg;
            //this.ProjectileSpeed = projectileSpd;
            //this.AttackSpeed = attackSpd;
            //this.ReloadDuration = reloadTime;
            //this.Projectiles = new List<Projectile>();
            //this.SpreadAmount = spreadAmount;
            //this.CanShoot = true;
            //this.ProjectileSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Bullet));
            //this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.GunAk47);
            //this.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
        }

        public void DrawProjectiles(RenderWindow gameWindow)
        {
            for (int i = 0; i < Projectiles.Count; i++)
            {
                if(Projectiles[i] != null)
                {
                    gameWindow.Draw(Projectiles[i]);
                }
            }
        }
        public void UpdateProjectiles(float deltaTimeInSeconds)
        {
            for (int i = 0; i < Projectiles.Count; i++)
            {
                if (Projectiles[i] != null)
                {
                    Projectile p = Projectiles[i];
                    if (p.TimeSinceCreation > p.DespawnBulletAfter)
                    {
                        Projectiles.RemoveAt(i);
                    }
                    else
                    {
                        p.AddDeltaTime(deltaTimeInSeconds);
                        p.Translate();
                    }
                }
            }
        }
        public override void Pickup(Player player)
        {
            bool check = false;
            for (int i = 0; i < 3; i++)
            {
                if (player.HoldingWeapon[i] != null && player.HoldingWeapon[i].Name == this.Name)
                {
                    check = true;
                }
            }
            if (check == false)
            {
                for (int i = 1; i < player.HoldingWeapon.Length; i++)
                {
                    if (player.HoldingWeapon[i] == null)
                    {
                        player.SetWeapon(this);
                        player.HoldingWeapon[i] = this;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">cursor position - player position</param>
        public void Shoot(Vector2f target, Player player)
        {
            this.Shoot(target, this.Position, this.Rotation, player, true);
        }


        /// <summary>
        /// Shoot method with starting conditions to compensate for network delay
        /// </summary>
        /// <param name="target">Where is bulet traveling to</param>
        /// <param name="shootOrgin">Where shot was fired</param>
        /// <param name="shootRotation">How was weapon rotated during shoot</param>
        public void Shoot(Vector2f target, Vector2f shootOrgin, float shootRotation, Player player, bool sendToServer)
        {
            if (Ammo != 0 && Reloading != true && ShootTimer.ElapsedTime.AsMilliseconds() > AttackSpeed )
            {
                ShootTimer.Restart();

                // maybe figure out how to make this Vector2f -> Vector2 -> Vector2f mess cleaner later
                const float projectileSpeed = 10f;
                Vector2 normalizedTarget = new Vector2(target.X, target.Y);
                normalizedTarget = Vector2.Normalize(normalizedTarget);
                Vector2f myTarget = new Vector2f(normalizedTarget.X, normalizedTarget.Y);
                Projectile bullet = new Projectile(myTarget * projectileSpeed, ProjectileSprite, shootOrgin, shootRotation);
                Projectiles.Add(bullet);
                ChangeAmmo(-1);


                Sound sound = ResourceHolderFacade.GetInstance().Sounds.Get(SoundIdentifier.GenericGun);
                sound.Volume = ResourceHolderFacade.GetInstance().CurrentVolume.GetVolume();
                sound.Play();

                if (sendToServer)
                {
                    var shootEventData = new ShootEventData(player.ToDTO(), target, this.Position, this.Rotation);
                    GameState.GetInstance().ConnectionManager.Connection.SendAsync("ShootEventServer", shootEventData);
                }
            }
        }


        public void CheckCollisions(Player shooter)
        {
            // 2 sets of loops, because we cannot modify the list while we are iterating through it
            List<int> indexesToRemove = new List<int>();

            var collidables = GameState.GetInstance().Collidables;
            var players = GameState.GetInstance().Players;


            for (int i = 0; i < Projectiles.Count; i++)
            {
                for(int j = 0; j < players.Count; j++)
                {
                    if (    !players[j].Name.Equals(shooter.Name) 
                        &&  !players[j].IsDead 
                        &&  CollisionTester.BoundingBoxTest(players[j], Projectiles[i].ProjectileSprite))
                    {
                        indexesToRemove.Add(i);

                        if (shooter.IsMainPlayer) {
                            players[j].AddHealth(-this.Damage);
                            GameState.GetInstance().ConnectionManager.Connection.SendAsync("UpdateScoresServer", shooter.ToDTO(), players[j].ToDTO());
                        }

                    }
                }

                for (int j = 0; j < collidables.Count; j++)
                {
                    if (CollisionTester.BoundingBoxTest(collidables[j], Projectiles[i].ProjectileSprite))
                    {
                        indexesToRemove.Add(i);

                        if (collidables[j] is HealthCrate)
                        {
                            Pickupable health = ((HealthCrate)collidables[j]).Pickupable;
                            //explosion.ExplosionCheck();
                            health.Position = collidables[j].Position;
                            //GameState.GetInstance().Pickupables.Add(health);
                            GameState.GetInstance().PickupableRep.GetIterator().Add(health);
                            collidables.RemoveAt(j);
                        }
                        else if (collidables[j] is ItemCrate)
                        {
                            Pickupable gun = ((ItemCrate)collidables[j]).Pickupable;
                            gun.Position = collidables[j].Position;
                            //GameState.GetInstance().Pickupables.Add(gun);
                            GameState.GetInstance().PickupableRep.GetIterator().Add(gun);
                            collidables.RemoveAt(j);

                        }

                    }
                }
            }

            bool OptimisedCollisionBullet(Sprite sprite, Sprite bullet) 
            {
                Vector2f colidablePos = new Vector2f(sprite.Position.X + 32, sprite.Position.Y + 32);

                double squaredDist = VectorUtils.GetSquaredDistance(colidablePos, bullet.Position);

                if(squaredDist < 4096)
                {
                    return CollisionTester.BoundingBoxTest(sprite, bullet);
                }

                return false;
            }

            // Edge case: remove duplicate collisions
            indexesToRemove = indexesToRemove.Distinct().ToList();

            // iterate from the end because indexes shift when we modify the list
            for (int i = indexesToRemove.Count-1; i >= 0; i--) 
            {
                if (Projectiles.Count != 0)
                {
                    Projectiles.RemoveAt(indexesToRemove[i]);
                }
            }
        }

        public void Reload()
        {
            if (this.Ammo != this.MagazineSize && this.Reloading != true)
            {
                Sound sound = SoundHolder.GetInstance().Get(SoundIdentifier.Reload);
                sound.Volume = SoundVolume.GetInstance().GetVolume();
                sound.Play();

                this.ChangeAmmo(-this.Ammo);
                this.ReloadTimer.Restart();
                this.Reloading = true;
            }
        }

        public void ReloadGunAnimation()
        {
            int elapsed = this.ReloadTimer.ElapsedTime.AsMilliseconds();

            SetAmmoPercent(elapsed / this.ReloadDuration * 100);
            
            if (this.Ammo >= this.MagazineSize && elapsed >= this.ReloadDuration)
            {
                OurLogger.Log("Gun finished reloading!");
                this.Reloading = false;
            }
        }

        public void ChangeAmmo(int ammo)
        {
            this.Ammo = CheckAmmoBounds(this.Ammo + ammo);
        }

        public void SetAmmoPercent(double p)
        {
            this.Ammo = CheckAmmoBounds((int)Math.Ceiling(this.MagazineSize * (p / 100d)));
        }

        public int CheckAmmoBounds(int ammo)
        {
            if (ammo < 0) return 0;
            if (ammo > this.MagazineSize) return MagazineSize;

            return ammo;
        }

        public IWeaponPrototype Clone()
        {
            Weapon copy = (Weapon) this.MemberwiseClone();
            //Weapon copy = this.DeepClone<Weapon>();
            copy.Projectiles = new List<Projectile>();
            copy.ReloadTimer = new Clock();
            copy.ShootTimer = new Clock();
            copy.ReloadCooldown = new Clock();
            //copy.ProjectileSprite = new Sprite(this.ProjectileSprite);
            //if(this.LaserSprite != null)
            //{
            //    copy.LaserSprite = new Sprite(this.LaserSprite);
            //}
            return copy;
        }

        public static Weapon CreateWeapon(WeaponType wepType)
        {
            return wepType switch
            {
                WeaponType.AssaultRifle => new AssaultRifle(),
                WeaponType.Pistol => new Pistol(),
                WeaponType.FlameThrower => new Flamethrower(),
                WeaponType.Minigun => new Minigun(),
                WeaponType.SniperRifle => new SniperRifle(),
                WeaponType.Shootgun => new Shotgun(),
                /* Default */ _ => new Pistol(),
            };
        }

    }
}
