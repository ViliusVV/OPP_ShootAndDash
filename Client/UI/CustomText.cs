using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using Client.Utilities;
using SFML.Graphics;
using SFML.System;

namespace Client.UI
{
    public class CustomText : Text
    {
        public CustomText(Font font, uint textSize)
        {
            this.Font = font;
            this.CharacterSize = textSize;
            this.Origin = new Vector2f(-5, -15f);
            this.OutlineThickness = 2;
            this.OutlineColor = Color.Black;
        }

        public CustomText(uint textSize)
        {
            this.Font = ResourceHolderFacade.GetInstance().Fonts.Get(FontIdentifier.PixelatedSmall);
            this.CharacterSize = textSize;
            this.OutlineThickness = 2;
            this.OutlineColor = Color.Black;
            this.Origin = new Vector2f(-5, -15f);
        }

    }
}
