using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;

namespace Client.Objects.Indestructables
{
    class Wall : Indestructable
    {
        private Sprite wallObject;

        public Wall()
        {

        }

        public override Sprite SpawnObject()
        {
            return wallObject;
        }
    }
}
