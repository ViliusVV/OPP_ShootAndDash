using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Utilities
{
	class TileMap : Drawable
	{
        public bool load(Vector2u tileSize, int[] tiles, uint width, uint height)
        {

            // resize the vertex array to fit the level size
            // m_vertices.PrimitiveType(Quads);
            m_vertices.Resize(width * height * 4);

            // populate the vertex array, with one quad per tile
            for (uint i = 0; i < width; ++i)
            {
                for (uint j = 0; j < height; ++j)
                {
                    // get the current tile number
                    int tileNumber = tiles[i + j * width];

                    // find its position in the tileset texture
                    long tu = tileNumber % (m_tileset.Size.X / tileSize.X);
                    long tv = tileNumber / (m_tileset.Size.X / tileSize.X);

                    // get a pointer to the current tile's quad
                    uint index = (i + j * width) * 4;

                    // define its 4 corners
                    m_vertices[index + 0] = new Vertex(new Vector2f(i * tileSize.X, j * tileSize.Y), new Vector2f(tu * tileSize.X, tv * tileSize.Y));
                    m_vertices[index + 1] = new Vertex(new Vector2f((i + 1) * tileSize.X, j * tileSize.Y), new Vector2f((tu + 1) * tileSize.X, tv * tileSize.Y));
                    m_vertices[index + 2] = new Vertex(new Vector2f((i + 1) * tileSize.X, (j + 1) * tileSize.Y), new Vector2f((tu + 1) * tileSize.X, (tv + 1) * tileSize.Y));
                    m_vertices[index + 3] = new Vertex(new Vector2f(i * tileSize.X, (j + 1) * tileSize.Y), new Vector2f(tu * tileSize.X, (tv + 1) * tileSize.Y));
                }
            }

            return true;
        }

        void Drawable.Draw(RenderTarget target, RenderStates states)
        {
            // apply the transform
            m_vertices.PrimitiveType = PrimitiveType.Quads;
            //states.Transform *= getTransform();

            // apply the tileset texture
            states.Texture = m_tileset;

            // draw the vertex array
            target.Draw(m_vertices, states);
        }

        public Texture m_tileset = new Texture("Assets/tilemap.png");
        
        private VertexArray m_vertices = new VertexArray();
        

    }
}
