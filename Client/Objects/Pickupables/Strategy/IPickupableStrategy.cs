using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Pickupables.Strategy
{
    interface IPowerUpStrategy
    {
        public void DoPowerUpLogic(Player player);
    }
}
