using Client.Config;
using Client.Models;
using Client.UI;
using Client.Utilities;
using Common.Utilities;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
namespace Client.Objects
{
    class Weapon : Pickupable
    {
        public string Name { get; private set; }
        public int MagazineSize { get; private set; }
        public int Ammo { get; private set; }
        public float Damage { get; private set; }
        public float ProjectileSpeed { get; private set; }
        public float AttackSpeed { get; private set; }
        public float ReloadTime { get; private set; }
        public int SpreadAmount { get; private set; }
        public bool CanShoot { get; private set; }
        public bool Reloading { get; set; }
        public List<Projectile> Projectiles {get;set;}

        public Clock WeaponClock { get; set; } = new Clock();
        public Sprite ProjectileSprite { get; private set; }


        public Weapon(string name, int magazineSize, int ammo, float dmg, float projectileSpd,
            float attackSpd, float reloadTime, int spreadAmount, bool canShoot)
        {
            this.Name = name;
            this.MagazineSize = magazineSize;
            this.Ammo = ammo;
            this.Damage = dmg;
            this.ProjectileSpeed = projectileSpd;
            this.AttackSpeed = attackSpd;
            this.ReloadTime = reloadTime;
            this.SpreadAmount = spreadAmount;
            this.CanShoot = canShoot;
            this.ProjectileSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Bullet));
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.GunAk47);
            this.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
        }

        public override void Pickup(Player player)
        {
            player.SetWeapon(this);
        }

        public void Shoot()
        {
            if (Ammo != 0 && Reloading != true)
            {
                //Vector2f v = VectorUtils.AngleDegToUnitVector(this.Rotation);

                //Sprite bulletSprite = new Sprite(ProjectileSprite);
                //Projectile bullet = new Projectile(Ve, target.Y * 1000, bulletSprite);

                //bullet.ProjectileSprite.Rotation = this.Rotation;
                //bullet.InitializeSpriteParams(SpriteUtils.GetSpriteCenter(bulletSprite), this.Position);


                //Projectiles.Add(bullet);
                //AmmoConsume(-1);

                //Sound sound = SoundHolder.GetInstance().Get(SoundIdentifier.GenericGun);
                //sound.Play();
            }

            //List<Projectile> projectiles = new List<Projectile>();
            //for (int i = 0; i < bulletCount; i++)
            //{
            //    Vector2 target = new Vector2(
            //        cursorPos.X - playerPos.X,
            //        cursorPos.Y - playerPos.Y
            //    );
            //    target.X += GameApplication.Rnd.Next(SpreadAmount);
            //    target.Y += GameApplication.Rnd.Next(SpreadAmount);
            //    target = Vector2.Normalize(target);
            //    Projectile bullet = new Projectile(target.X * ProjectileSpeed,
            //        target.Y * ProjectileSpeed, ProjectileSprite);
            //    bullet.InitializeSpriteParams(SpriteUtils.GetSpriteCenter(ProjectileSprite), playerPos);
            //    bullet.ProjectileSprite.Rotation = VectorUtils.VectorToAngle(target.X, target.Y);
            //    Ammo--;
            //    projectiles.Add(bullet);
            //}

            //// play sound here
            ////    bulletList.Add(bullet);
            ////    Sound sound = Sounds.Get(SoundIdentifier.GenericGun);
            //return projectiles;
        }

        public void AmmoConsume(int i)
        {
            this.Ammo += i;
        }
    }
}
