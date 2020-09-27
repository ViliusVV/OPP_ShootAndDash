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
        public Sprite Projectile { get; private set; }

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
            this.Projectile = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Bullet));
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
                    target.X += GameApplication.Rnd.Next(SpreadAmount);
                    target.Y += GameApplication.Rnd.Next(SpreadAmount);
                    target = Vector2.Normalize(target);
                    Projectile bullet = new Projectile(target.X * ProjectileSpeed,
                        target.Y * ProjectileSpeed, Projectile);
                    bullet.InitializeSpriteParams(SpriteUtils.GetSpriteCenter(Projectile), playerPos);
                    bullet.ProjectileSprite.Rotation = VectorUtils.VectorToAngle(target.X, target.Y);
                    Ammo--;
                    projectiles.Add(bullet);
                }
            // play sound here
            //    bulletList.Add(bullet);
            //    Sound sound = Sounds.Get(SoundIdentifier.GenericGun);
            return projectiles;
        }
        public void AmmoConsume(int i)
        {
            this.Ammo += i;
        }
        public float GetAmmo(float scale)
        {
            
            if (Ammo > 0)
            {
                return scale * (float)Ammo / (float)MagazineSize;
            }
            else
            {
                return 0;
            }
        }
        public float AmmoOffSet(float scale)
        {
            if (Ammo > 0)
            {
                return (float)15.5 * ((MagazineSize - Ammo) * scale / MagazineSize);
            }
            else
            {
                return 0;
            }
        }
    }
}
