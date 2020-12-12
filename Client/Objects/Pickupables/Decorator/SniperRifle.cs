using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using SFML.Graphics;
using Client.Utilities;
using SFML.System;
using Client.Objects.Pickupables.Mediator;

namespace Client.Objects.Pickupables.Decorator
{
    public class SniperRifle : WeaponDecorator
    {
        public SniperRifle(IMediator mediator) : base(mediator)
        {
            this.Name = "Sniper";
            this.MagazineSize = 5;
            this.Ammo = 5;
            this.Damage = 80;
            this.ProjectileSpeed = 2000;
            this.AttackSpeed = 500;
            this.ReloadDuration = 2000;
            this.Projectiles = new List<Projectile>();
            this.SpreadAmount = 50;
            this.CanShoot = true;
            this.ProjectileSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Bullet));
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.SniperRifle);
            this.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
        }
    }
}

//new SniperRifle("Sniper", 5, 60, 2000, 500, 3000, 50);