using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class PlayerAnimation
    {
        private Player player;

        public IntRect playerAnimationBounds = new IntRect(36, 0, 36, 64);
        public readonly IntRect playerIdleBounds = new IntRect(0, 0, 36, 64);
        private Clock animationClock = new Clock();

        public PlayerAnimation(Player player){
            this.player = player;
            this.player.TextureRect = playerAnimationBounds;
        }

        public void Update()
        {
            // Run player animation
            if (this.animationClock.ElapsedTime.AsSeconds() > 0.05f && this.player.Running)
            {
                if (this.playerAnimationBounds.Left == 144)
                {
                    this.playerAnimationBounds.Left = 36;
                }
                else
                {
                    this.playerAnimationBounds.Left += 36;
                }
                  
                this.player.TextureRect = playerAnimationBounds;
                this.animationClock.Restart();
            }
            else if (!this.player.Running)
            {
                this.player.TextureRect = playerIdleBounds;
            }
        }
    }
}
