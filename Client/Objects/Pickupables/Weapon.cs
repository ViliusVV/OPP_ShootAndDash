using Client.Config;
using Client.Models;
using Client.UI;
using Client.Utilities;
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
        public float Damage { get; private set; }
        public float ProjectileSpeed { get; private set; }
        public float AttackSpeed { get; private set; }
        public float ReloadTime { get; private set; }
        public int SpreadAmount { get; private set; }
        public bool CanShoot { get; private set; }
        public Sprite Projectile { get; private set; }

        public Weapon(string name, int magazineSize, float dmg, float projectileSpd,
            float attackSpd, float reloadTime, int spreadAmount, bool canShoot, Sprite projectile)
        {
            this.Name = name;
            this.MagazineSize = magazineSize;
            this.Damage = dmg;
            this.ProjectileSpeed = projectileSpd;
            this.AttackSpeed = attackSpd;
            this.ReloadTime = reloadTime;
            this.SpreadAmount = spreadAmount;
            this.CanShoot = canShoot;
            this.Projectile = projectile;
        }
        public override void Pickup(Player player)
        {
            player.SetWeapon(this);
        }
        public void SetProjectileSprite(Sprite projectileSprite)
        {
            this.Projectile = projectileSprite;
        }
        public List<Projectile> Shoot(int bulletCount, Vector2 cursorPos, Vector2f playerPos)
        {
            List<Projectile> projectiles = new List<Projectile>();

            for (int i = 0; i < bulletCount; i++)
            {
                Vector2 target = new Vector2(
                cursorPos.X - playerPos.X,
                cursorPos.Y - playerPos.Y
                );
                target.X += GameApplication.rnd.Next(SpreadAmount);
                target.Y += GameApplication.rnd.Next(SpreadAmount);
                target = Vector2.Normalize(target);
                Projectile bullet = new Projectile(target.X * ProjectileSpeed,
                    target.Y * ProjectileSpeed, Projectile);
                bullet.InitializeSpriteParams(SpriteUtils.GetSpriteCenter(Projectile), playerPos);
                bullet.ProjectileSprite.Rotation = VectorUtils.VectorToAngle(target.X, target.Y);

                projectiles.Add(bullet);
            }
            // play sound here
            //    bulletList.Add(bullet);
            //    Sound sound = Sounds.Get(SoundIdentifier.GenericGun);
            return projectiles;
        }
    }
}
