using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using SFML.Graphics;
using Client.Utilities;

namespace Client.Objects.Pickupables.Decorator
{
    class SniperRifle : Weapon
    {
        public SniperRifle(string name, int magazineSize, float dmg, float projectileSpd,
float attackSpd, float reloadTime, int spreadAmount) : base(name, magazineSize, dmg, projectileSpd, attackSpd,
reloadTime, spreadAmount)
        {
            GetTexture();
            //this.decoratedWeapon = DecoratedWeapon;
            //this.decoratedWeapon.Texture 
        }

        public void GetTexture()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.SniperRifle);
        }
    }
}
