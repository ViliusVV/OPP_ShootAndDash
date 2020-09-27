using Client.Models;
using Client.Objects;
using Common;
using Common.DTO;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers
{
    class GameState
    {
        private static readonly GameState _instance = new GameState();
        public List<Player> Players { get; set; }
        public List<Pickupable> Pickupables { get; set; }
        public List<Sprite> Collidables { get; set; }
        private GameState()
        {
            this.Players = new List<Player>();
            this.Pickupables = new List<Pickupable>();
            this.Collidables = new List<Sprite>();
        }
        public static GameState GetInstance()
        {
            return _instance;
        }

        public void ToDTO()
        {
            GameStateDTO dto = new GameStateDTO();
            foreach(Player player in Players)
            {
                dto.Players.Add(player.ToDTO());
            }
        }
        
        public void FromDTO(GameStateDTO dto)
        {
            foreach(PlayerDTO playerDto in dto.Players)
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
