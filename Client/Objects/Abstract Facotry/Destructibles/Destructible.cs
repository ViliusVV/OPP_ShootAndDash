﻿using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;
using SFML.System;

namespace Client.Objects.Destructables
{
    abstract class Destructible : Sprite
    {
        public abstract Sprite SpawnObject();
        public abstract Sprite DestroyBehavior(Vector2f position);
        
    }
}