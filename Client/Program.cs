using System;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

using System.Collections.Generic;
using Client.Objects;
using System.Numerics;

using Client.Utilities;
using Client.UI;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            GameApplication game = new GameApplication();

            Console.WriteLine("Starting game...");

            game.Run();

            Console.WriteLine("Game closed!");
        }
    }
}
