using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.PlayerState
{
    class IdleState : IPlayerState
    {
        public bool IsDead { get => false; }
        public bool IsRunning { get => false; }
        public bool IsLocked { get => false; }

        private Player _context;

        public IdleState(Player context)
        {
            this._context = context;
        }

        public void Animate()
        {
            this._context.TextureRect = this._context.PlayerAnimation.playerIdleBounds;
            this.TryToSwitchState();
        }


        public void TryToSwitchState()
        {
            if (this._context.CheckSpeed())
            {
                this._context.SetRunningState();
            }
        }
    }
}