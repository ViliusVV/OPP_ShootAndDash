using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers.Iterator.Repositories
{
    public class PlayerRepository : Container
    {
        public static List<Player> Players;
        public Iterator GetIterator()
        {
            return new PlayerIterator();
        }
        private class PlayerIterator : Iterator
        {
            int index;
            public PlayerIterator()
            {
                index = 0;
            }

            public void Add(Object obj)
            {
                if (Players == null)
                    Players = new List<Player>();
                Players.Add((Player)obj);
            }

            public void First()
            {
                index = 0;
            }

            public bool HasNext()
            {
                if (index < Players.Count)
                    return true;
                return false;
            }

            public object Next()
            {
                return Players[index++];
            }

            public void Remove()
            {
                if (Players[index - 1] != null)
                {
                    Players.RemoveAt(--index);
                }
            }

            public void RemoveAt(int index)
            {
                Players.RemoveAt(index);
            }
        }
    }
}
