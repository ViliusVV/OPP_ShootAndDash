using Client.Config;
using Client.Objects.Pickupables.Decorator;
using Client.Utilities;
using Common.Utilities;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Template
{
    class FreezeTrapBuilder : TrapSpawner
    {
        public FreezeTrapBuilder()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Minigun);
        }

        public override Sprite ApplySkin()
        {
            return new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Minigun));
        }

        public override float ApplyDamage()
        {
            return 0f;
        }

        public override void ApplyBehavior()
        {
            this.Rotation = 90;
        }
    }
}
