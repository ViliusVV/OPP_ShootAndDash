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

        public Dictionary<string, ServerPlayer> Players { get; set; }

        public GameManager() {
            Players = new Dictionary<string, ServerPlayer>();
        }
        public static GameManager GetInstance()
        {
            return _instance;
        }

        public ServerGameState GetGameStateDTO()
        {
            ServerGameState dto = new ServerGameState();
            for (int i = 0; i < Players.Values.Count; i++)
            {
                dto.Players.Add(Players.Values.ToList()[i]);
            }

            return dto;
        }
    }
}
