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
            Console.ForegroundColor = ConsoleColor.White;
            GameApplication game = GameApplication.GetInstance();

            GameApplication.defaultLogger.LogMessage(10, "Starting game...");

            game.Run();

            GameApplication.defaultLogger.LogMessage(10, "Game closed!");
        }
    }
}
