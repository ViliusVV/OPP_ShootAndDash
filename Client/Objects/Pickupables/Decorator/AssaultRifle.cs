using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using SFML.Graphics;
using Client.Utilities;
using SFML.System;

namespace Client.Objects.Pickupables.Decorator
{
    class AssaultRifle : WeaponDecorator
    {
        public AssaultRifle() : base()
        {
            this.Name = "AK-47";
            this.MagazineSize = 45;
            this.Ammo = 45;
            this.Damage = 25;
            this.ProjectileSpeed = 2000;
            this.AttackSpeed = 90;
            this.ReloadDuration = 800;
            this.Projectiles = new List<Projectile>();
            this.SpreadAmount = 50;
            this.CanShoot = true;
            this.ProjectileSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Bullet));
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.GunAk47);
            this.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
        }
    }
}
