using Client.Models;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers.Iterator.Repositories
{
    public class PlayerRepository : Container
    {
        public static List<Player> Players;
        public IIterator GetIterator()
        {
            return new PlayerIterator();
        }
        public IIterator GetIterator(double distance, Sprite player)
        {
            return new PlayerIterator(distance, player);
        }
        private class PlayerIterator : IIterator
        {
            List<Player> players;
            int index;
            public PlayerIterator()
            {
                index = 0;
                players = Players;
            }
            public PlayerIterator(double dist, Sprite target)
            {
                index = 0;
                players = Players.FindAll(x => IsPlayerInRange(x, target, dist));
            }
            public void Add(object obj)
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
                if (index < players.Count)
                    return true;
                return false;
            }

            public object Next()
            {
                return players[index++];
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
            public bool IsPlayerInRange(Sprite player1, Sprite player2, double range)
            {
                var dist = MathF.Sqrt(Common.Utilities.VectorUtils.GetSquaredDistance(player1.Position, player2.Position));
                return dist < range;
            }
        }
    }
}
