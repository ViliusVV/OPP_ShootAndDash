using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    class Player: Sprite
    {
        public int Health { get; set; }

        public Vector2f Speed { get; set; } = new Vector2f(0.0f, 0.0f);

        //// Configure text
        //Font font = Fonts.Get(FontIdentifier.PixelatedSmall);
        //Text text = new Text("000 000", font)
        //{
        //    CharacterSize = 14,
        //    OutlineThickness = 2.0f

        //};
        //float textWidth = text.GetLocalBounds().Width;
        //float textHeight = text.GetLocalBounds().Height;
        //float xOffset = text.GetLocalBounds().Left + 30;
        //float yOffset = text.GetLocalBounds().Top + 30;
        //text.Origin = new Vector2f(textWidth / 2f + xOffset, textHeight / 2f + yOffset);
        //text.Position = new Vector2f(position.X, position.Y);
    }
}
