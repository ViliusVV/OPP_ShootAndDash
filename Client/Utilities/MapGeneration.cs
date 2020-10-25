using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using Client.Managers;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;

namespace Client.Utilities
{
	class MapGeneration
	{
        //public TileMap map = new TileMap();

        GameState GameState = GameState.GetInstance();

        //---Collidables list----

        Sprite crate;
        Sprite crate2;
        Sprite bushSprite;

        ////---------------------

        //Choose textures for collidables
        public void LoadTextures()
        {
            crate = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.CrateBrown));
            crate2 = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.CrateBrown));
            bushSprite = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Bush));
        }

        //Choose positions for collidables
        public void LoadPositions()
        {
            crate.Position = new Vector2f(1000, 400);
            crate2.Position = new Vector2f(1000, 500);
            bushSprite.Position = new Vector2f(500, 400);
        }

        //Add Collidable
        public void AddCollidables()
        {
            GameState.Collidables.Add(crate);
            GameState.Collidables.Add(crate2);
            GameState.Collidables.Add(bushSprite);
        }


        public void PrepareMap()
        {
            //map.CreateMap();
            LoadTextures();
            LoadPositions();
            AddCollidables();
        }

	}
}
