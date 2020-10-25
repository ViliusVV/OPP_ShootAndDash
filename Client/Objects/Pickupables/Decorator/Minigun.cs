using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using SFML.Graphics;
using Client.Utilities;
using SFML.System;

namespace Client.Objects.Pickupables.Decorator
{
    public class Minigun : WeaponDecorator
    {
        public Minigun() : base()
        {
            this.Name = "Minigun";
            this.MagazineSize = 50;
            this.Ammo = 50;
            this.Damage = 30;
            this.ProjectileSpeed = 2000;
            this.AttackSpeed = 50;
            this.ReloadDuration = 5000;
            this.Projectiles = new List<Projectile>();
            this.SpreadAmount = 50;
            this.CanShoot = true;
            this.ProjectileSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Bullet));
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Minigun);
            this.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
        }
    }
}
//new Minigun("Minigun", 50, 30, 2000, 50, 5000, 50);