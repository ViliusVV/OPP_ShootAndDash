using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Memento
{
    class Memento
    {
        private Vector2f pos;

        public Memento(Vector2f pos)
        {
            this.pos = pos;
        }

        public Vector2f Pos
        {
            get { return pos; }
        }

    }
}
