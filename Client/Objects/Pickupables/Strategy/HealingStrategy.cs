using Client.Models;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Pickupables.Strategy
{
    public class HealingStrategy : IPowerUpStrategy
    {
        private float HealAmount { get; set; } = 30f;

        public HealingStrategy() { }

        public HealingStrategy(float healAmount)
        {
            this.HealAmount = healAmount;
        }

        public void DoPowerUpLogic(Player player)
        {
            OurLogger.Log("Executing healing powerup strategy");
            player.AddHealth(HealAmount);
        }
    }
}
