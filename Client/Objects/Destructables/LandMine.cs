using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;

namespace Client.Objects.Destructables
{
    class LandMine : Destructable
    {
        private Sprite landMineObject;

        public LandMine()
        {

        }

        public override Sprite SpawnObject()
        {
            return landMineObject;
        }
    }
}
