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
    }
}
