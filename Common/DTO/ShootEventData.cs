using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    public class ShootEventData
    {
        public ServerPlayer Shooter { get; set; }

        public Vector2fS Target { get; set; }
        public Vector2fS Orgin { get; set; }
        public float Rotation { get; set; }

        public ShootEventData() { }

        public ShootEventData(ServerPlayer shooter, Vector2f target, Vector2f orgin, float rotation)
        {
            this.Shooter = shooter;
            this.Target = target;
            this.Orgin = orgin;
            this.Rotation = rotation;
        }

        public override string ToString()
        {
            return String.Format($"Shooter:{Shooter.Name} ,Tgt: {Target}, Org: {Orgin}, Rot:{Rotation}");
        }
    }
}
