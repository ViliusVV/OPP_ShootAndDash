using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Memento
{
    class Memento
    {
        private Vector2f pos;
        private Texture tex;

        public Memento(Vector2f pos, Texture tex)
        {
            this.pos = pos;
            this.tex = tex;
        }

        public Vector2f Pos
        {
            get { return pos; }
        }

        public Texture Tex
        {
            get { return tex; }
        }

    }
}
