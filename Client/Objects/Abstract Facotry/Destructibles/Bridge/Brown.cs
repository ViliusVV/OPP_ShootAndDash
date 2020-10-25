using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using Client.Config;
using Client.Utilities;
using SFML.System;
using Client.Objects.Destructables;

namespace Client.Objects.Abstract_Facotry.Destructibles.Bridge
{
    class Brown : IColorSelection
    {
        public IColorSelection checkModel(string text, Destructible destr)
        {
            if (text == "Medkit")
            {
                destr.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.MedkitCrateBrown);
                return this;
            }
            else if (text == "Item")
            {
                destr.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.CrateBrown);
                return this;
            }
            return null;
        }
    }
}
