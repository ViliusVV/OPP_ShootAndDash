using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Memento
{
    class Caretaker
    {
        private Memento memento;

        public Memento Memento
        {
            set { memento = value; }
            get { return memento; }
        }
    }
}
