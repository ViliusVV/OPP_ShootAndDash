﻿using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers.Iterator
{
    public interface Container
    {
        public IIterator GetIterator();
        public IIterator GetIterator(double distance, Sprite target);
    }
}
