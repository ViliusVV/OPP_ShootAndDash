using Client.Objects.Pickupables.Decorator;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Client.Utilities;
using SFML.System;
using Client.Models;
using Common.Utilities;
using Client.UI;

namespace Client.Objects.Pickupables.Mediator
{
    class ConcreteMediator : IMediator
    {
        private Text text;

        private AssaultRifle assaultRifle;
        private Flamethrower flamethrower;
        private Minigun minigun;
        private Shotgun shotgun;
        private SniperRifle sniperRifle;

        private AssaultRifle assaultRifleGreen;
        private AssaultRifle assaultRifleRed;
        private SniperRifle sniperRifleGreen;
        private SniperRifle sniperRifleRed;

        private List<Weapon> weaponsList;

        public ConcreteMediator()
        {
            minigun = new Minigun(this);
            assaultRifle = new AssaultRifle(this);
            flamethrower = new Flamethrower(this);
            shotgun = new Shotgun(this);
            sniperRifle = new SniperRifle(this);

            assaultRifleGreen = new AssaultRifle(this);
            new GreenLaser(assaultRifleGreen, this);
            assaultRifleRed = new AssaultRifle(this);
            new RedLaser(assaultRifleRed, this);
            
            sniperRifleGreen = new SniperRifle(this);
            new GreenLaser(sniperRifleGreen, this);
            sniperRifleRed = new SniperRifle(this);
            new RedLaser(sniperRifleRed, this);

            weaponsList = new List<Weapon>();
            weaponsList.Add(minigun);
            weaponsList.Add(assaultRifle);
            weaponsList.Add(flamethrower);
            weaponsList.Add(shotgun);
            weaponsList.Add(sniperRifle);
            weaponsList.Add(assaultRifleGreen);
            weaponsList.Add(assaultRifleRed);
            weaponsList.Add(sniperRifleGreen);
            weaponsList.Add(sniperRifleRed);
        }

        public void Send(string message, Weapon weapon)
        {
            foreach (Weapon wep in weaponsList)
            {
                if (wep.Name == weapon.Name && message == "pickedup")
                {
                    text.DisplayedString = "Picked up " + weapon.Name;
                    OurLogger.Log("Picked up " + weapon.Name);
                }
                else if (wep.Name == weapon.Name && message == "dropped")
                {
                    text.DisplayedString = "Dropped " + weapon.Name;
                    OurLogger.Log("Dropped " + weapon.Name);
                }
            }
        }

        public void GetPlayerText(PlayerBar bar)
        {
            text = bar.WeaponText;
        }
    }
}
