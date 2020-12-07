using Client.Objects;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers.Iterator.Repositories
{
    public class PickupableRepository : Container
    {
        public static List<Pickupable> Pickupables;
        public IIterator GetIterator()
        {
            return new PickupableIterator();
        }
        public IIterator GetIterator(double distance, Sprite target)
        {
            return new PickupableIterator(distance, target);
        }
        private class PickupableIterator : IIterator
        {
            List<Pickupable> pickupables;
            int index;
            public PickupableIterator()
            {
                index = 0;
                pickupables = Pickupables;
            }
            public PickupableIterator(double dist, Sprite target)
            {
                index = 0;
                pickupables = Pickupables.FindAll(x => IsInRange(x, target, dist));
            }
            public void Add(object obj)
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
                if (index < pickupables.Count)
                    return true;
                return false;
            }

            public object Next()
            {
                return pickupables[index++];
            }

            public void Remove()
            {
                if(Pickupables[index-1] != null)
                {
                    Pickupables.RemoveAt(--index);
                }
            }

            public void RemoveAt(int index)
            {
                Pickupables.RemoveAt(index);
            }
            public bool IsInRange(Sprite target1, Sprite target2, double range)
            {
                var dist = MathF.Sqrt(Common.Utilities.VectorUtils.GetSquaredDistance(target1.Position, target2.Position));
                return dist < range;
            }
        }
    }
}
