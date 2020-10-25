using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using Client.Objects.Abstract_Facotry.Destructibles.Bridge;
using SFML.Graphics;
using SFML.System;

namespace Client.Objects.Destructables
{
    abstract class Destructible : Sprite
    {
        public IItemBridge ItemBridge;
        public abstract Sprite SpawnObject();
        
    }
}
