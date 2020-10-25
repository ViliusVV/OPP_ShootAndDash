using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using Client.Config;
using Client.Utilities;
using SFML.System;

namespace Client.Objects.Abstract_Facotry.Destructibles.Bridge
{
    class Brown : Color
    {
        public override Sprite SpawnObject()
        {
            return this;
        }
        public void checkModel(string text)
        {
            if (text == "Medkit")
            {
                this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.MedkitCrateBrown);
            }
            else if (text == "Item")
            {
                this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.CrateBrown);
            }
        }
    }
}
