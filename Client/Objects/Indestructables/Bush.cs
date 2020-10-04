using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;

namespace Client.Objects.Indestructables
{
    class Bush : Indestructable
    {
        private Sprite bushObject;

        public Bush()
        {

        }

        public override Sprite SpawnObject()
        {
            return bushObject;
        }
    }
}
