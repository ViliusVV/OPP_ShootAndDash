using Client.Models;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Objects.Pickupables.Strategy
{
    class MovmentSpeedStrategy : IPowerUpStrategy
    {
        private float SpeedMultiplier { get; set; } = 2;
        private int Duration { get; set; } = 2000;

        public MovmentSpeedStrategy() { }
        public MovmentSpeedStrategy(float speedMultiplier, int duration)
        {
            this.SpeedMultiplier = SpeedMultiplier;
            this.Duration = duration;
        }
        public void DoPowerUpLogic(Player player)
        {
            OurLogger.Log("Executing movement speed powerup strategy");

            player.SpeedMultiplier = SpeedMultiplier;

            Task.Delay((int)Duration).ContinueWith(o => player.SpeedMultiplier = 1);
        }
    }
}
