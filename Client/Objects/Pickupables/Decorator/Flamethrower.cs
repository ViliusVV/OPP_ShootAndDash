using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using SFML.Graphics;
using Client.Utilities;
using SFML.System;

namespace Client.Objects.Pickupables.Decorator
{
    class Flamethrower : WeaponDecorator
    {
        public Flamethrower() : base()
        {
            this.Name = "FlameThrower";
            this.MagazineSize = 200;
            this.Ammo = 200;
            this.Damage = 10;
            this.ProjectileSpeed = 500;
            this.AttackSpeed = 20;
            this.ReloadDuration = 2000;
            this.Projectiles = new List<Projectile>();
            this.SpreadAmount = 50;
            this.CanShoot = true;
            this.ProjectileSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Bullet));
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Flamethrower);
            this.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
        }
    }
}

//new Flamethrower("Flamethrower", 200, 10, 500, 100, 20, 100);