using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.PlayerState
{
    public interface IPlayerState
    {
        public bool IsDead { get; }
        public bool IsRunning { get;  }
        public bool IsLocked { get;  }

        public void Animate();

        public void HandleDeath() { }

        public void TryToSwitchState();
    }
}
