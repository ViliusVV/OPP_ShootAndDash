using Common;
using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class GameManager
    {
        private static readonly GameManager _instance = new GameManager();

        public Dictionary<string, PlayerDTO> Players { get; set; }

        public GameManager() {
            Players = new Dictionary<string, PlayerDTO>();
        }
        public static GameManager GetInstance()
        {
            return _instance;
        }

        public GameStateDTO GetGameStateDTO()
        {
            GameStateDTO dto = new GameStateDTO();
            for (int i = 0; i < Players.Values.Count; i++)
            {
                dto.Players.Add(Players.Values.ToList()[i]);
            }

            return dto;
        }
    }
}
