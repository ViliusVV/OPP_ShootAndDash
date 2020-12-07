using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Memento
{
    class Caretaker
    {
        private Memento _memento;

        public Memento Memento
        {
            set { _memento = value; }
            get { return _memento; }
        }
    }
}
