using Common.Enums;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ModelInterfaces
{
    interface IProjectile
    {
        public int Damage { get; set; }
        public Vector2f Posiotion { get; set; }
        public Vector2f Speed { get; set; }
    }
}
