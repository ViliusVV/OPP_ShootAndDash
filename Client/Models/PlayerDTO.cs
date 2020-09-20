using Client.Config;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    class PlayerDTO
    {
        public float Health { get; private set; } = 100;

        public Vector2f Speed { get; set; } = new Vector2f(0.0f, 0.0f);
        public Vector2f Position { get; set; } = new Vector2f(0.0f, 0.0f);

        public bool IsDead { get; private set; } = false;
        public float SpeedMultiplier { get; private set; } = 1;
        public TextureIdentifier TextureIdentifier { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Position.X, Position.Y);
        }
    }
}
