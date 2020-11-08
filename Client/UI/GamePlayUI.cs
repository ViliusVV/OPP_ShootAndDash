using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI
{
    class GamePlayUI
    {
        public Scoreboard Scoreboard { get; set; }
        public CustomText RespawnMesage { get; set; }
        public KillNotifier KillNotifier { get; set; } = new KillNotifier();
        // public KillMessages {get;set}
    }
}
