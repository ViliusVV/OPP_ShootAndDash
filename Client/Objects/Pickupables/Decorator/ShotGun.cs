using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using SFML.Graphics;
using Client.Utilities;
using SFML.System;

namespace Client.Objects.Pickupables.Decorator
{
    class Shotgun : WeaponDecorator
    {
        public Shotgun() : base()
        {
            this.Name = "Shotgun";
            this.MagazineSize = 5;
            this.Ammo = 5;
            this.Damage = 18;
            this.ProjectileSpeed = 2000;
            this.AttackSpeed = 1000;
            this.ReloadDuration = 700;
            this.Projectiles = new List<Projectile>();
            this.SpreadAmount = 50;
            this.CanShoot = true;
            this.ProjectileSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Bullet));
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Shotgun);
            this.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
        }
    }
}

//new Shotgun("Shotgun", 50, 5, 2000, 100, 1000, 50);