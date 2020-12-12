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
                    spawn = new Minigun(GameApplication.GetInstance().m);
                    break;
                case 1:
                    spawn = new SniperRifle(GameApplication.GetInstance().m);
                    break;
                case 2:
                    spawn = new Flamethrower(GameApplication.GetInstance().m);
                    break;
                case 3:
                    spawn = new Shotgun(GameApplication.GetInstance().m);
                    break;
                // Weapons with laser
                case 4:
                    spawn = new SniperRifle(GameApplication.GetInstance().m);
                    new RedLaser(spawn, GameApplication.GetInstance().m);
                    break;
                case 5:
                    spawn = new AssaultRifle(GameApplication.GetInstance().m);
                    new RedLaser(spawn, GameApplication.GetInstance().m);
                    break;
                case 6:
                    spawn = new SniperRifle(GameApplication.GetInstance().m);
                    new GreenLaser(spawn, GameApplication.GetInstance().m);
                    break;
                case 7:
                    spawn = new AssaultRifle(GameApplication.GetInstance().m);
                    new GreenLaser(spawn, GameApplication.GetInstance().m);
                    break;
                default:
                    spawn = new AssaultRifle(GameApplication.GetInstance().m);
                    break;
            }
            return spawn;
        }
    }
}
