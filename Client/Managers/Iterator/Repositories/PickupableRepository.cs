using Client.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers.Iterator.Repositories
{
    public class PickupableRepository : Container
    {
        public static List<Pickupable> Pickupables;
        public Iterator GetIterator()
        {
            return new PickupableIterator();
        }

        private class PickupableIterator : Iterator
        {
            int index;
            public PickupableIterator()
            {
                index = 0;
            }

            public void Add(Object obj)
            {
                if (Pickupables == null)
                    Pickupables = new List<Pickupable>();
                Pickupables.Add((Pickupable)obj);
            }

            public void First()
            {
                index = 0;
            }

            public bool HasNext()
            {
                if (index < Pickupables.Count)
                    return true;
                return false;
            }

            public object Next()
            {
                return Pickupables[index++];
            }

            public void Remove()
            {
                if(Pickupables[index-1] != null)
                {
                    Pickupables.RemoveAt(--index);
                }
            }
        }
    }
}
