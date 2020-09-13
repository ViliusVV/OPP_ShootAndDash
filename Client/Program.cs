﻿using System;
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
        static void Main()
        {
            GameApplication game = GameApplication.Instance;

            Console.WriteLine("Starting game...");

            game.Run();

            Console.WriteLine("Game closed!");
        }
    }
}
