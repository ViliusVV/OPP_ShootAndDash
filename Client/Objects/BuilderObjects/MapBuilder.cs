using Client.Config;
using Client.Managers;
using Client.Utilities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.BuilderObjects
{
    class MapBuilder : IBuilder
    {
        TileMap Map;
        GameState GameState = GameState.GetInstance();

        Sprite Wall;

        //Choose textures for collidables
        public void LoadTextures()
        {
            Wall = new Sprite(TextureHolder.GetInstance().Get(TextureIdentifier.Wall));
        }

        public IBuilder BuildBuilding()
        {
            for (int i = 10; i < 21; i++)
            {
                Sprite crate = new Sprite(Wall);
                crate.Position = new Vector2f(64 * i, 64 * 10);
                GameState.Collidables.Add(crate);

                crate = new Sprite(Wall);
                crate.Position = new Vector2f(64 * i, 64 * 20);
                GameState.Collidables.Add(crate);
            }
            for (int i = 11; i < 20; i++)
            {
                if (i == 15 || i == 16)
                    continue;
                Sprite crate = new Sprite(Wall);
                crate.Position = new Vector2f(64*10, 64 * i);
                GameState.Collidables.Add(crate);

                crate = new Sprite(Wall);
                crate.Position = new Vector2f(64*20, 64 * i);
                GameState.Collidables.Add(crate);
            }
            return this;
        }

        public IBuilder BuildWalls()
        {
            for (int i = 0; i < Map.Length; i++)
            {
                Sprite crate = new Sprite(Wall);
                crate.Position = new Vector2f(64 * i, 0);
                GameState.Collidables.Add(crate);

                crate = new Sprite(Wall);
                crate.Position = new Vector2f(64 * i, 64 * (Map.Width - 1));
                GameState.Collidables.Add(crate);
            }
            for (int i = 1; i < Map.Width - 1; i++) 
            {
                Sprite crate = new Sprite(Wall);
                crate.Position = new Vector2f(0, 64 * i);
                GameState.Collidables.Add(crate);

                crate = new Sprite(Wall);
                crate.Position = new Vector2f(64 * (Map.Length - 1), 64 * i);
                GameState.Collidables.Add(crate);
            }
            return this;
        }

        public IBuilder StartNew(int length, int width)
        {
            Map = new TileMap();
            Map.CreateMap(length, width);
            return this;
        }
        public IBuilder Reset()
        {
            GameState.Collidables = new List<Sprite>();
            return this;
        }
        public TileMap GetResult()
        {
            return Map;
        }
    }
}
