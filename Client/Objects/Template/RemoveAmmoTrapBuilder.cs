﻿using Client.Config;
using Client.Models;
using Client.Objects.Pickupables.Decorator;
using Client.Utilities;
using Common.Utilities;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Objects.Template
{
    class RemoveAmmoTrapBuilder : TrapSpawner
    {
        public RemoveAmmoTrapBuilder()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.RemoveAmmoTrap);
        }

        public override Sprite ApplySkin()
        {
            return new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.RemoveAmmoTrap));
        }

        public override void ApplyDamage(Player player)
        {
            //OurLogger.Log("Stepped on Remove Ammo trap");
            GameApplication.defaultLogger.LogMessage(6, "Stepped on remove ammo trap");

            player.AddHealth(-5f);
        }

        public override void ApplyBehavior(Player player)
        {
            if (player.SpeedMultiplier > 0.9f)
            {
                player.SpeedMultiplier = 0.8f;
                Task.Delay(100).ContinueWith(o => player.SpeedMultiplier = 1);
            }
            player.Weapon.Ammo = 0;
        }
    }
}
