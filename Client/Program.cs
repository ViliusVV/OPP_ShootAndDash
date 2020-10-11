using System;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

using System.Collections.Generic;
using Client.Objects;
using System.Numerics;

using Client.Utilities;
using Client.UI;
using Common.Utilities;

namespace Client
{
    class Program
    {
        static void Main()
        {
            GameApplication game = GameApplication.GetInstance();

            OurLogger.Log("Starting game...");

            game.Run();

            OurLogger.Log("Game closed!");
        }
    }
}
