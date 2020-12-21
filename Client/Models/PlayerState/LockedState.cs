using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.PlayerState
{
    class LockedState : IPlayerState
    {
        public bool IsDead { get => false; }
        public bool IsRunning { get => false; }
        public bool IsLocked { get => true; }

        private Player _context;


        public LockedState(Player context)
        {
            this._context = context;
        }


        public void Animate()
        {

        }

        public void UpdateSpeed()
        {

        }

        public void TryToSwitchState() { }
    }
}
