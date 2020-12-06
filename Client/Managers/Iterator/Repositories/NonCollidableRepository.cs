using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers.Iterator.Repositories
{
    public class NonCollidableRepository : Container
    {
        public static List<Sprite> NonCollidables;
        public IIterator GetIterator()
        {
            return new NonCollidableIterator();
        }
        private class NonCollidableIterator : IIterator
        {
            int index;
            public NonCollidableIterator()
            {
                index = 0;
            }

            public void Add(object obj)
            {
                if (NonCollidables == null)
                    NonCollidables = new List<Sprite>();
                NonCollidables.Add((Sprite)obj);
            }

            public void First()
            {
                index = 0;
            }

            public bool HasNext()
            {
                if (index < NonCollidables.Count)
                    return true;
                return false;
            }

            public object Next()
            {
                return NonCollidables[index++];
            }

            public void Remove()
            {
                if (NonCollidables[index - 1] != null)
                {
                    NonCollidables.RemoveAt(--index);
                }
            }

            public void RemoveAt(int index)
            {
                NonCollidables.RemoveAt(index);
            }
        }
    }
}
