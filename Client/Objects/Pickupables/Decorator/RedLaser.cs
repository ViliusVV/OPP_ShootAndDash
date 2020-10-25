using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using SFML.Graphics;
using Client.Utilities;
using SFML.System;

namespace Client.Objects.Pickupables.Decorator
{
	public class RedLaser : LaserDecorator
	{
        public RedLaser(Weapon newweapon) : base()
        {
            newweapon.LaserSight = new string("Red");
            newweapon.LaserSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.RedLaser));
            CheckGun(newweapon.Name, newweapon);
            //this.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
        }
        public void CheckGun(string text, Weapon newweapon)
        {
            //if (text == "Pistol")
            //{
            //    newweapon.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.RedPistolLaser);
            //    newweapon.LaserSprite.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
            //}
            if (text == "AK-47")
            {
                newweapon.Name = "AK-47R";
                newweapon.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.RedGunAk47Laser);
                newweapon.LaserSprite.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 8f);
            }
            else if (text == "Sniper")
            {
                newweapon.Name = "SniperG";
                newweapon.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.RedSniperLaser);
                newweapon.LaserSprite.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 10f);
            }
        }
    }
}
