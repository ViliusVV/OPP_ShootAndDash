using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using SFML.Graphics;
using Client.Utilities;
using SFML.System;

namespace Client.Objects.Pickupables.Decorator
{
    class Pistol : Weapon
    {
        public Pistol(string name, int magazineSize, float dmg, float projectileSpd,
float attackSpd, float reloadTime, int spreadAmount) : base(name, magazineSize, dmg, projectileSpd, attackSpd,
reloadTime, spreadAmount)
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Pistol);
            this.Origin = new Vector2f(SpriteUtils.GetSpriteCenter(this).X, 3f);
            //this.decoratedWeapon = DecoratedWeapon;
            //this.decoratedWeapon.Texture 
        }
    }
}
