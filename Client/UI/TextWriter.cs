using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace Client.UI
{
    class CustomText : Text
    {
        public CustomText(Font font, uint textSize)
        {
            this.Font = font;
            this.CharacterSize = textSize;
            this.Origin = new Vector2f(0, -20f);
            this.OutlineThickness = 2;
            this.OutlineColor = Color.Black;
        }
    }
}
