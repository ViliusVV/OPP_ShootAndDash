using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using Client.Models;
using Client.Objects.Pickupables.Strategy;
using Client.Utilities;

namespace Client.Objects.Pickupables
{
    class ReloadSyringe : PowerUp
    {
        public ReloadSyringe()
        {
            this.PowerUpStrategy = new ReloadSpeedStrategy();
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.ReloadSyringe);
        }
    }
}
