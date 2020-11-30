using Client.Config;
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
    class FreezeTrapBuilder : TrapSpawner
    {
        public FreezeTrapBuilder()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.FreezeTrap);
        }

        public override Sprite ApplySkin()
        {
            return new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.FreezeTrap));
        }

        public override void ApplyDamage(Player player)
        {
            OurLogger.Log("Stepped on Freeze trap");
            player.AddHealth(-5f);
        }

        public override void ApplyBehavior(Player player)
        {
            player.SpeedMultiplier = 0.2f;
            Task.Delay(2000).ContinueWith(o => player.SpeedMultiplier = 1);
        }
    }
}
