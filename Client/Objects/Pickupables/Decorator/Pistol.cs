using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using SFML.Graphics;
using Client.Utilities;
using SFML.System;

namespace Client.Objects.Pickupables.Decorator
{
    public class Pistol : WeaponDecorator
    {
        public Pistol() : base()
        {
            this.Name = "Pistol";
            this.MagazineSize = 20;
            this.Ammo = 20;
            this.Damage = 18;
            this.ProjectileSpeed = 2000;
            this.AttackSpeed = 200;
            this.ReloadDuration = 500;
            this.Projectiles = new List<Projectile>();
            this.SpreadAmount = 50;
            this.CanShoot = true;
            this.ProjectileSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Bullet));
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Pistol);
            this.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
        }
    }
}
