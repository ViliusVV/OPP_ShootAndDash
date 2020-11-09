using Client.Models;
using Client.Objects;
using Client.Objects.Destructables;
using Client.Objects.Indestructables;
using Client.Utilities;
using Common;
using Common.DTO;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers
{
    public class GameState
    {
        private static GameState _instance = new GameState();

        public Random Random { get; private set; }

        public List<Player> Players { get; set; }
        public List<Pickupable> Pickupables { get; set; }
        public virtual List<Sprite> Collidables { get; set; }
        public TileMap TileMap { get; set; }
        public List<Sprite> NonCollidables { get; set; }

        public ConnectionManager ConnectionManager { get; set; }


        public GameState()
        {
            this.Players = new List<Player>();
            this.Pickupables = new List<Pickupable>();
            this.Collidables = new List<Sprite>();
            this.NonCollidables = new List<Sprite>();
            this.Random = new Random();
        }

        public static GameState GetInstance()
        {
            return _instance;
        }

        public static void SetTestingInstance(GameState instance)
        {
            _instance = instance;
        }

        public void InitRandom(int seed)
        {
            this.Random = new Random(seed);
        }


        public void ToDTO()
        {
            ServerGameState dto = new ServerGameState();
            foreach(Player player in Players)
            {
                dto.Players.Add(player.ToDTO());
            }
        }
        
        public void FromDTO(ServerGameState dto)
        {
            foreach(ServerPlayer playerDto in dto.Players)
            {
                Player player = Players.Find(p => p.Name.Equals(playerDto.Name));

                if (player != null && !player.IsMainPlayer)
                {
                    player.RefreshData(playerDto);
                }
            }
        }
    }
}
