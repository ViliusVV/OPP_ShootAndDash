﻿using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using SFML.Graphics;
using Client.Utilities;
using SFML.System;
using Client.Objects.Pickupables.Mediator;

namespace Client.Objects.Pickupables.Decorator
{
	public class GreenLaser : LaserDecorator
	{
        public GreenLaser(Weapon newweapon, IMediator mediator) : base(mediator)
        {
            newweapon.LaserSight = new string("Green");
            newweapon.LaserSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.GreenLaser));
            CheckGun(newweapon.Name, newweapon);
            //newweapon.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
        }
        public void CheckGun(string text, Weapon newweapon)
		{
   //         if(text == "Pistol")
			//{
   //             texture.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.GreenPistolLaser);
   //             texture.LaserSprite.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
   //         } 
            if (text == "AK-47")
			{
                newweapon.Name = "AK-47G";
                newweapon.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.GreenGunAk47Laser);
                newweapon.LaserSprite.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 8f);
            } 
            else if (text == "Sniper")
			{
                newweapon.Name = "SniperG";
                newweapon.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.GreenSniperLaser);
                newweapon.LaserSprite.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 10f);
            }
		}
    }
}
