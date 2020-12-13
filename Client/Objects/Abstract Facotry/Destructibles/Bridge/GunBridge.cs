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
                    spawn = new Minigun(GameApplication.GetInstance().mediator);
                    break;
                case 1:
                    spawn = new SniperRifle(GameApplication.GetInstance().mediator);
                    break;
                case 2:
                    spawn = new Flamethrower(GameApplication.GetInstance().mediator);
                    break;
                case 3:
                    spawn = new Shotgun(GameApplication.GetInstance().mediator);
                    break;
                // Weapons with laser
                case 4:
                    spawn = new SniperRifle(GameApplication.GetInstance().mediator);
                    new RedLaser(spawn, GameApplication.GetInstance().mediator);
                    break;
                case 5:
                    spawn = new AssaultRifle(GameApplication.GetInstance().mediator);
                    new RedLaser(spawn, GameApplication.GetInstance().mediator);
                    break;
                case 6:
                    spawn = new SniperRifle(GameApplication.GetInstance().mediator);
                    new GreenLaser(spawn, GameApplication.GetInstance().mediator);
                    break;
                case 7:
                    spawn = new AssaultRifle(GameApplication.GetInstance().mediator);
                    new GreenLaser(spawn, GameApplication.GetInstance().mediator);
                    break;
                default:
                    spawn = new AssaultRifle(GameApplication.GetInstance().mediator);
                    break;
            }
            return spawn;
        }
    }
}
