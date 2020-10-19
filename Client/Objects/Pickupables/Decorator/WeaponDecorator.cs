using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Pickupables.Decorator
{
    abstract class WeaponDecorator : Weapon
    {

        public WeaponDecorator(string name, int magazineSize, float dmg, float projectileSpd,
            float attackSpd, float reloadTime, int spreadAmount) : base(name, magazineSize, dmg, projectileSpd, attackSpd,
                reloadTime, spreadAmount)
        {
        }


    }
}
