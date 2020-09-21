using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    [Serializable()]
    public class PlayerDTO
    {
        public string Name { get; set; }
        public float Health { get; set; } = 100;

        public Vector2fSerializable Speed { get; set; }
        public Vector2fSerializable Position { get; set; }

        public bool IsDead { get; private set; } = false;
        public float SpeedMultiplier { get; set; } = 1;
        public string Appearance { get; set; }

        public PlayerDTO() { }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Name, Position.X, Position.Y);
        }
    }
}
