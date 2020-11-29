using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers.Iterator
{
    public interface Iterator
    {
        public bool HasNext();
        public Object Next();
        public void First();
        public void Remove();
        public void Add(Object obj);
    }
}
