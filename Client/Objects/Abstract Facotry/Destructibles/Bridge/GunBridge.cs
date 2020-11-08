using System;
using System.Collections.Generic;
using System.Text;
using Client.Managers;
using Client.Objects.Pickupables.Decorator;

namespace Client.Objects.Abstract_Facotry.Destructibles.Bridge
{
    class GunBridge : IItemBridge
    {
        public Pickupable GetPickupable()
        {
            int num = GameState.GetInstance().Random.Next(8);
            Weapon spawn;
            switch (num)
            {
                case 0:
                    spawn = new Minigun();
                    break;
                case 1:
                    spawn = new SniperRifle();
                    break;
                case 2:
                    spawn = new Flamethrower();
                    break;
                case 3:
                    spawn = new Shotgun();
                    break;
                // Weapons with laser
                case 4:
                    spawn = new SniperRifle();
                    new RedLaser(spawn);
                    break;
                case 5:
                    spawn = new AssaultRifle();
                    new RedLaser(spawn);
                    break;
                case 6:
                    spawn = new SniperRifle();
                    new GreenLaser(spawn);
                    break;
                case 7:
                    spawn = new AssaultRifle();
                    new GreenLaser(spawn);
                    break;
                default:
                    spawn = new AssaultRifle();
                    break;
            }
            return spawn;
        }
    }
}
