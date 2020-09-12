using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    class Gun : Sprite
    {
        public float AttackRate { get; set; }
        public float ReloadTime { get; set; }
        public int Damage { get; set; }
        public int AmmoCapacity { get; set; }

        public Gun() { }

    }
}
