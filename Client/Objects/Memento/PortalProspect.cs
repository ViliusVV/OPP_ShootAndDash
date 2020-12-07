using Client.Config;
using Client.Utilities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Memento
{
    class PortalProspect : PortalPickupCheck
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



        public Memento CreateMemento()
        {
            return new Memento(pos);
        }

        public void SetMemento(Memento memento)
        {
            this.Pos = memento.Pos;
        }
    }
}
