using Client.Config;
using Client.Utilities;
using Common.Utilities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Memento
{
    class PortalProspect : Sprite
    {
        private Vector2f pos;
        public Vector2f Pos
        {
            get { return pos; }
            set
            {
                pos = value;
                this.Position = this.Pos;
            }
        }

        private Texture tex;
        public Texture Tex
        {
            get { return tex; }
            set
            {
                tex = value;
                this.Texture = this.Tex;
            }
        }

        public Memento CreateMemento()
        {
            OurLogger.Log("-- MEMENTO CREATED --");
            return new Memento(pos, tex);
        }

        public void RestoreMemento(Memento memento)
        {
            OurLogger.Log("-- MEMENTO RESTORED --");
            this.Pos = memento.Pos;
            this.Tex = memento.Tex;
        }
    }
}
