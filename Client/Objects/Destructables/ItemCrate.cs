using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;

namespace Client.Objects.Destructables
{
    class ItemCrate : Destructable
    {
        private Sprite itemCrateObject;

        public ItemCrate()
        {

        }

        public override Sprite SpawnObject()
        {
            return itemCrateObject;
        }
    }
}
