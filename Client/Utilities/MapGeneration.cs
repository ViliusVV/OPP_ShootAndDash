using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;

namespace Client.Utilities
{
	class MapGeneration
	{
        public TileMap map = new TileMap();
        public void CreateMap()
        {
            int[] level = new int[]
            {
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            };

            Vector2u vect = new Vector2u(64, 64);
            if (!map.load(vect, level, 16, 8))
            {
                Console.WriteLine("error on map load");
            };
        }

	}
}
