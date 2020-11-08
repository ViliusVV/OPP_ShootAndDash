using Client.Objects;
using Client.Objects.Pickupables.Decorator;
using Common.DTO;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Client.Adapters
{
    public class ServerWeaponAdapter: ServerWeapon
    {
        private readonly Weapon _weapon;

        public override String WeaponName { get => _weapon.Name; }
        public override int Ammo { get => _weapon.Ammo; }
        public override WeaponType WeaponType { get => GetType(_weapon); }

        public ServerWeaponAdapter(Weapon weapon)
        {
            this._weapon = weapon;
        }

        public static WeaponType GetType(Weapon weapon)
        { 
            switch (weapon)
            {
                case Pistol _:
                    return WeaponType.Pistol;
                case AssaultRifle _:
                    return WeaponType.AssaultRifle;
                case Shotgun _:
                    return WeaponType.Shootgun;
                case Minigun _:
                    return WeaponType.Minigun;
                case SniperRifle _:
                    return WeaponType.SniperRifle;
                case Flamethrower _:
                    return WeaponType.FlameThrower;
                default:
                    return WeaponType.Pistol;
            }
        }
    }
}
