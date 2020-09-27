﻿using SFML.System;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Common.DTO
{
    [Serializable()]
    public class PlayerDTO
    {

        public string Name { get; set; }
        public float Health { get; set; } = 100;

        public float MaxSpeed { get; set; }
        public Vector2fS Speed { get; set; }
        public Vector2fS Position { get; set; }

        public bool IsDead { get; set; } = false;
        public float SpeedMultiplier { get; set; } = 1;
        public string Appearance { get; set; }

        // List of all projectiles that player has shot and currently is in world

        public PlayerDTO() { }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Name, Position.X, Position.Y);
        }
    }
}
