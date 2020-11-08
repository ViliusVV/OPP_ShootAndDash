using Common.Enums;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Common.DTO
{
    [Serializable()]
    public class ServerPlayer
    {

        public string Name { get; set; }
        public float Health { get; set; } = 100;
        public bool IsDead { get; set; } = false;

        public int Kills { get; set; }
        public int Deaths { get; set; }

        public Vector2fS Speed { get; set; }
        public Vector2fS Position { get; set; }
        public float Heading { get; set; }

        public PlayerSkinType Appearance { get; set; }

        public ServerWeapon ServerWeapon { get; set; }

        // List of all projectiles that player has shot and currently is in world

        public ServerPlayer() { }

        public override string ToString()
        {
            return string.Format($"Nick: {Name}, HP: {Health}, Speed: {Speed}, Pos: {Position}, Dead: {IsDead}, Skin: {Appearance}");
        }
    }
}
