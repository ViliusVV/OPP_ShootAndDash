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
        public IIterator GetIterator(double distance, Sprite target)
        {
            return new NonCollidableIterator(distance, target);
        }
        private class NonCollidableIterator : IIterator
        {
            List<Sprite> nonCollidables;
            int index;
            public NonCollidableIterator()
            {
                index = 0;
                nonCollidables = NonCollidables;
            }
            public NonCollidableIterator(double dist, Sprite target)
            {
                index = 0;
                nonCollidables = NonCollidables.FindAll(x => IsInRange(x, target, dist));
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
                if (index < nonCollidables.Count)
                    return true;
                return false;
            }

            public object Next()
            {
                return nonCollidables[index++];
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
            public bool IsInRange(Sprite player1, Sprite player2, double range)
            {
                var dist = MathF.Sqrt(Common.Utilities.VectorUtils.GetSquaredDistance(player1.Position, player2.Position));
                return dist < range;
            }
        }
    }
}
