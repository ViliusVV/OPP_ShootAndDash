using Client.Config;
using Client.Utilities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI
{
    class PlayerBar: Drawable
    {
        public float BarScale { get; } = 1.5f;

        public Sprite PlayerBarSprite { get; set; }
        public Sprite PlayerHealtMaskSprite { get; set; }
        public Sprite PlayerAmmoMaskSprite { get; set; }

        public Vector2f Position 
        {
            get => PlayerBarSprite.Position;
            set {
                PlayerBarSprite.Position = value;
                PlayerHealtMaskSprite.Position = value;
                PlayerAmmoMaskSprite.Position = value;
            } 
        }

        public PlayerBar()
        {
            var textures = TextureHolder.GetInstance();

            PlayerBarSprite = new Sprite(textures.Get(TextureIdentifier.PlayerBar));
            PlayerHealtMaskSprite = new Sprite(textures.Get(TextureIdentifier.PlayerBarMask));
            PlayerAmmoMaskSprite = new Sprite(textures.Get(TextureIdentifier.PlayerBarAmmoMask));

            ConfigBar();
        }

        public void ConfigBar()
        {
            PlayerBarSprite.Origin = SpriteUtils.GetSpriteCenter(PlayerBarSprite);
            PlayerHealtMaskSprite.Origin = SpriteUtils.GetSpriteCenter(PlayerHealtMaskSprite);
            PlayerAmmoMaskSprite.Origin = SpriteUtils.GetSpriteCenter(PlayerAmmoMaskSprite);

            Vector2f barScaleVec = new Vector2f(BarScale, BarScale);
            PlayerBarSprite.Scale = barScaleVec;
            PlayerHealtMaskSprite.Scale = barScaleVec;
            PlayerAmmoMaskSprite.Scale = barScaleVec;
        }

        public void Update(float healtPercent, float ammoPercent)
        {
            
            //PlayerBar.PlayerHealtMaskSprite.Position = new Vector2f(mainPlayer.Position.X - mainPlayer.HealthOffSet(1.5f), mainPlayer.Position.Y - 40);
            //PlayerBar.PlayerAmmoMaskSprite.Position = new Vector2f(mainPlayer.Position.X - wep.AmmoOffSet(1.5f), mainPlayer.Position.Y - 40);

            PlayerHealtMaskSprite.Scale = new Vector2f(healtPercent / 100f * BarScale, BarScale);
            //PlayerHealtMaskSprite.Position = new Vector2f(PlayerHealtMaskSprite.Position.X - mainPlayer.HealthOffSet(1.5f), PlayerHealtMaskSprite.Position.Y - 40);
            PlayerAmmoMaskSprite.Scale = new Vector2f(ammoPercent / 100f * BarScale, BarScale);
        }


        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(PlayerHealtMaskSprite);
            target.Draw(PlayerAmmoMaskSprite);
            target.Draw(PlayerBarSprite);
        }
    }
}
