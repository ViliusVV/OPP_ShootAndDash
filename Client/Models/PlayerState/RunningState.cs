using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.PlayerState
{
    class RunningState : IPlayerState
    {

        public bool IsDead { get => false; }
        public bool IsRunning { get => true; }
        public bool IsLocked { get => false; }

        private Player _context;
        private bool _debounce = false;

        public RunningState(Player context)
        {
            this._context = context;
        }


        public void Animate()
        {
            this._context.PlayerAnimation.Update();
            this.TryToSwitchState();
        }


        public void TryToSwitchState()
        {
            if (!this._context.CheckSpeed())
            {
                this._context.SetIdleState();
            }
        }
    }
}
