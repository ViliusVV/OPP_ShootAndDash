﻿using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;

namespace Client.Objects.Indestructables
{
    abstract class Indestructable : Sprite
    {
        public abstract Sprite SpawnObject();
    }
}
