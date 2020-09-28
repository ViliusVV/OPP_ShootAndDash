using Client.Models;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects
{
    public abstract class Pickupable : Sprite
    {
        public bool PickedUp = false;
        public abstract void Pickup(Player player);
    }
}
