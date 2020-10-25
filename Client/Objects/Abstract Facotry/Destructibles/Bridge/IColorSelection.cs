using Client.Objects.Destructables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Abstract_Facotry.Destructibles.Bridge
{
    interface IColorSelection
    {
        IColorSelection checkModel(string text, Destructible destr);
    }
}
