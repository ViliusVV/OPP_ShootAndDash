using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ModelInterfaces
{
    interface IWeapon
    {
        public WeaponId GunId { get; set; }
        public int AmmoLeft { get; set; }
    }
}
