using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers.Iterator
{
    public interface IIterator
    {
        public bool HasNext();
        public object Next();
        public void First();
        public void Remove();
        public void RemoveAt(int index);
        public void Add(object obj);
    }
}
