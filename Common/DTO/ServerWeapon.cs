using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    public class ServerWeapon
    {
        public virtual WeaponType WeaponType { get; set; }
        public virtual String WeaponName { get; set; }
        public virtual int Ammo { get; set; }
    }
}
