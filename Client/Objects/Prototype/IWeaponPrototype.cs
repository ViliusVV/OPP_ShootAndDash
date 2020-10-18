using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Prototype
{
    interface IWeaponPrototype
    {
        public IWeaponPrototype Clone();
    }
}
