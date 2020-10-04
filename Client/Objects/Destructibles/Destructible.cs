using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;

namespace Client.Objects.Destructables
{
    abstract class Destructible : Sprite
    {
        public abstract Sprite SpawnObject();
    }
}
