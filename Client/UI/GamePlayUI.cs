using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI
{
    class GamePlayUI
    {
        public Scoreboard Scoreboard { get; set; } = new Scoreboard();
        public CustomText RespawnMesage { get; set; } = new CustomText(7 * 5);
        public KillNotifier KillNotifier { get; set; } = new KillNotifier();

        public InGameLog InGameLog { get; set; } = new InGameLog();
        // public KillMessages {get;set}
    }
}
