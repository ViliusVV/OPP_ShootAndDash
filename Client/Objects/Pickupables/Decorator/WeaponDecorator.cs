using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Pickupables.Decorator
{
    abstract class WeaponDecorator
    {
        public Weapon decoratedWeapon;

        public WeaponDecorator(Weapon DecoratedWeapon)
        {
            this.decoratedWeapon = DecoratedWeapon;
            //this.decoratedWeapon.Texture 
        }


    }
}
