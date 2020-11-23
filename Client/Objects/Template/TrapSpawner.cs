using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Template
{
    abstract class TrapSpawner : Sprite
    {
        public abstract Sprite ApplySkin();
        public abstract float ApplyDamage();
        public abstract void ApplyBehavior();

        public void BuildTrap()
        {
            ApplySkin(); // think of something else? 
            ApplyDamage();
            ApplyBehavior();
        }
    }
}
