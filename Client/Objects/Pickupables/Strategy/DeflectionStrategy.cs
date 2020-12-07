using Client.Models;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Objects.Pickupables.Strategy
{
    class DeflectionStrategy : IPowerUpStrategy
    {
        private int Duration { get; set; } = 5000;

        public DeflectionStrategy() { }

        public DeflectionStrategy(int duration)
        {
            this.Duration = duration;
        }

        public void DoPowerUpLogic(Player player)
        {
            //OurLogger.Log("Executing deflection powerup strategy");
            GameApplication.defaultLogger.LogMessage(5, "Executing deflection powerup strategy");
            player.IsInvincible = true;

            Task.Delay((int)Duration).ContinueWith(o => player.IsInvincible = false);
        }
    }
}
