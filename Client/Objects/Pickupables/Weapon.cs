using Client.Collisions;
using Client.Config;
using Client.Models;
using Client.Objects.Prototype;
using Client.UI;
using Client.Utilities;
using Common.Utilities;
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
    class Weapon : Pickupable, IWeaponPrototype
    {
        public string Name { get; private set; }
        public int MagazineSize { get; private set; }
        public int Ammo { get; private set; }
        public float Damage { get; private set; }
        public float ProjectileSpeed { get; private set; }
        public float AttackSpeed { get; private set; }
        public float ReloadDuration { get; set; }
        public int SpreadAmount { get; private set; }
        public bool CanShoot { get; private set; }
        public bool Reloading { get; set; }
        public List<Projectile> Projectiles {get;set;}
        public Clock ShootTimer { get; set; } = new Clock();
        public Clock ReloadTimer { get; set; } = new Clock();
        public Clock ReloadCooldown { get; set; } = new Clock();
        public Sprite ProjectileSprite { get; private set; }

        public Weapon(string name, int magazineSize, float dmg, float projectileSpd,
            float attackSpd, float reloadTime, int spreadAmount)
        {
            this.Name = name;
            this.MagazineSize = magazineSize;
            this.Ammo = magazineSize;
            this.Damage = dmg;
            this.ProjectileSpeed = projectileSpd;
            this.AttackSpeed = attackSpd;
            this.ReloadDuration = reloadTime;
            this.Projectiles = new List<Projectile>();
            this.SpreadAmount = spreadAmount;
            this.CanShoot = true;
            this.ProjectileSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Bullet));
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.GunAk47);
            this.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
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
            player.SetWeapon(this);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">cursor position - player position</param>
        public void Shoot(Vector2f target)
        {
            if (Ammo != 0 && Reloading != true && ShootTimer.ElapsedTime.AsMilliseconds() > AttackSpeed )
            {
                ShootTimer.Restart();

                // maybe figure out how to make this Vector2f -> Vector2 -> Vector2f mess cleaner later
                const float projectileSpeed = 10f;
                Vector2 normalizedTarget = new Vector2(target.X, target.Y);
                normalizedTarget = Vector2.Normalize(normalizedTarget);
                Vector2f myTarget = new Vector2f(normalizedTarget.X, normalizedTarget.Y);
                Projectile bullet = new Projectile(myTarget * projectileSpeed, ProjectileSprite, this.Position, this.Rotation);
                Projectiles.Add(bullet);
                ChangeAmmo(-1);

                Sound sound = SoundHolder.GetInstance().Get(SoundIdentifier.GenericGun);
                sound.Play();
            }
        }
        public void CheckCollisions(List<Sprite> collidables)
        {
            // 2 sets of loops, because we cannot modify the list while we are iterating through it
            List<int> indexesToRemove = new List<int>();

            for (int i = 0; i < Projectiles.Count; i++)
            {
                for (int j = 0; j < collidables.Count; j++)
                {
                    if (CollisionTester.BoundingBoxTest(collidables[j], Projectiles[i].ProjectileSprite))
                    {
                        indexesToRemove.Add(i);
                    }
                }
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
            copy.Projectiles = new List<Projectile>();
            copy.ReloadTimer = new Clock();
            copy.ShootTimer = new Clock();
            copy.ReloadCooldown = new Clock();
            return copy;
        }
    }
}
