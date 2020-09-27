using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ModelInterfaces
{
    interface IPlayer
    {
        public string Name { get; set; }
        public float Health { get; set; }

        public Vector2f Speed { get; set; }
        public Vector2f Position { get; set; }

        public bool IsDead { get; set; }
    }
}
