using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using SFML.Graphics;
using Client.Utilities;
using SFML.System;

namespace Client.Objects.Pickupables.Decorator
{
	class RedLaser : LaserDecorator
	{
        public RedLaser(string name, int magazineSize, float dmg, float projectileSpd,
float attackSpd, float reloadTime, int spreadAmount) : base(name, magazineSize, dmg, projectileSpd, attackSpd,
reloadTime, spreadAmount)
        {
            this.LaserSight = new string("Red");
            this.LaserSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.RedLaser));
            CheckGun(name);
            this.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
        }
        public void CheckGun(string text)
        {
            if (text == "Pistol")
            {
                this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.RedPistolLaser);
                this.LaserSprite.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
            }
            else if (text == "AK-47")
            {
                this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.RedGunAk47Laser);
                this.LaserSprite.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 8f);
            }
            else if (text == "Sniper")
            {
                this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.RedSniperLaser);
                this.LaserSprite.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 10f);
            }
        }
    }
}
