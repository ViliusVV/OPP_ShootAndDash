using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;
using Client.Config;
using Client.Utilities;
using SFML.System;

namespace Client.Objects.Destructables
{
    class HealthCrate : Destructible
    {
        public HealthCrate()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.MedkitCrate);
        }

        public override Sprite SpawnObject()
        {
            return this;
        }

    }
}
