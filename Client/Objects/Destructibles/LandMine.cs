using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;
using Client.Config;
using Client.Utilities;

namespace Client.Objects.Destructables
{
    class LandMine : Destructible
    {
        private Sprite landMineObject;

        public LandMine()
        {
            landMineObject = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.LandMine));
        }

        public override Sprite SpawnObject()
        {
            Console.WriteLine("Land mine spawned");
            return landMineObject;
        }
    }
}
