using Client.Models;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Objects.Pickupables.Strategy
{
    class ReloadSpeedStrategy : IPowerUpStrategy
    {
        public float ReloadReduction { get; set; } = 5;
        public int Duration { get; set; } = 10000;

        public ReloadSpeedStrategy() { }

        public ReloadSpeedStrategy(float relaodReduction, int duration) {
            this.ReloadReduction = ReloadReduction;
            this.Duration = duration;
        }

        public void DoPowerUpLogic(Player player)
        {
            if(player.Weapon != null)
            {
                OurLogger.Log("Executing reload speed powerup strategy");

                float oldRelaodTime = player.Weapon.ReloadDuration;
                player.Weapon.ReloadDuration = oldRelaodTime / ReloadReduction;

                Task.Delay((int)Duration).ContinueWith(o => player.Weapon.ReloadDuration = oldRelaodTime);
            }
        }
    }
}
