using Client.Objects;
using Client.Objects.Pickupables.Decorator;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.PlayerState
{
    class DeadState: IPlayerState
    {
        public bool IsDead { get => true; }
        public bool IsRunning { get => false; }
        public bool IsLocked { get => true; }
        public Player _context;
        
        private bool _entryDead = true;


        public DeadState(Player context)
        {
            this._context = context;
        }

        public void Animate()
        {

        }

        public void HandleDeath()
        {
            GameApplication ga = GameApplication.GetInstance();

            if (_entryDead)
            {
                _entryDead = false;
                ga.RespawnTimer.Restart();
            }

            float elapsedDeath = ga.RespawnTimer.ElapsedTime.AsSeconds();
            var text = ga.GameplayUI.RespawnMesage;


            if (elapsedDeath > ga.deathTimeout)
            {
                ga.ForceSpawnObject(ga.MainPlayer);
                ga.MainPlayer.Health = 100;
                text.DisplayedString = "";

                _entryDead = true;
                ga.RespawnTimer.Restart();
                _context.AddHealth(100);
                _context.SetIdleState();
            }
            else
            {
                try
                {
                    text.DisplayedString = "You're dead. Respawning in " + (ga.deathTimeout - elapsedDeath).ToString("N2");
                    text.Origin = new Vector2f(text.GetLocalBounds().Left / 2f, text.GetLocalBounds().Top / 2f);
                    text.Position = new Vector2f(ga.GameWindow.GetViewport(ga.MainView).Height / 2f, ga.GameWindow.GetViewport(ga.MainView).Width / 2f);
                    
                }
                catch { }
            }
        }

        public void TryToSwitchState()
        {

        }
    }
}
