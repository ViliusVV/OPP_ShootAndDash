using Client.Models;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Template
{
    public abstract class TrapSpawner : Pickupable
    {
        public abstract Sprite ApplySkin();
        public abstract void ApplyDamage(Player player);
        public abstract void ApplyBehavior(Player player);

        public void BuildTrap()
        {
            ApplySkin();
        }

        public override void Pickup(Player player)
        {
            this.PickedUp = true;
            PlayerKnockback(player);

            ApplyDamage(player);
            ApplyBehavior(player);
        }

        void PlayerKnockback(Player player)
        {
            if (player.Speed.X > 0 && player.Speed.Y == 0) // Left 
            {
                player.Speed = new SFML.System.Vector2f(player.Speed.X - 20f, 0f);
            }
            else if (player.Speed.X < 0 && player.Speed.Y == 0) // Right 
            {
                player.Speed = new SFML.System.Vector2f(player.Speed.X + 20f, 0f);
            }
            else if (player.Speed.X == 0 && player.Speed.Y > 0) // Top
            {
                player.Speed = new SFML.System.Vector2f(0f, player.Speed.Y - 20f);
            }
            else if (player.Speed.X == 0 && player.Speed.Y < 0) // Bottom
            {
                player.Speed = new SFML.System.Vector2f(0f, player.Speed.Y + 20f);
            }
            else if (player.Speed.X > 0 && player.Speed.Y > 0) // Top Left
            {
                player.Speed = new SFML.System.Vector2f(player.Speed.X - 20f, player.Speed.Y - 20f);
            }
            else if (player.Speed.X < 0 && player.Speed.Y > 0) // Top Right
            {
                player.Speed = new SFML.System.Vector2f(player.Speed.X + 20f, player.Speed.Y - 20f);
            }
            else if (player.Speed.X > 0 && player.Speed.Y < 0) // Bottom Left
            {
                player.Speed = new SFML.System.Vector2f(player.Speed.X - 20f, player.Speed.Y + 20f);
            }
            else if (player.Speed.X < 0 && player.Speed.Y < 0) // Bottom Right
            {
                player.Speed = new SFML.System.Vector2f(player.Speed.X + 20f, player.Speed.Y + 20f);
            }
        }
    }
}
