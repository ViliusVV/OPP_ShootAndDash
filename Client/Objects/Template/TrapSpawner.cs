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
            PlayerKnockback(player, 20f);

            ApplyDamage(player);
            ApplyBehavior(player);
        }

        void PlayerKnockback(Player player, float power)
        {
            if (player.Speed.X > 0 && player.Speed.Y == 0) // Left 
            {
                player.Speed = new SFML.System.Vector2f(player.Speed.X - power, 0f);
                //player.Position = new SFML.System.Vector2f(player.Position.X - power, player.Position.Y);
            }
            else if (player.Speed.X < 0 && player.Speed.Y == 0) // Right 
            {
                player.Speed = new SFML.System.Vector2f(player.Speed.X + power, 0f);
            }
            else if (player.Speed.X == 0 && player.Speed.Y > 0) // Top
            {
                player.Speed = new SFML.System.Vector2f(0f, player.Speed.Y - power);
            }
            else if (player.Speed.X == 0 && player.Speed.Y < 0) // Bottom
            {
                player.Speed = new SFML.System.Vector2f(0f, player.Speed.Y + power);
            }
            else if (player.Speed.X > 0 && player.Speed.Y > 0) // Top Left
            {
                player.Speed = new SFML.System.Vector2f(player.Speed.X - power, player.Speed.Y - power);
            }
            else if (player.Speed.X < 0 && player.Speed.Y > 0) // Top Right
            {
                player.Speed = new SFML.System.Vector2f(player.Speed.X + power, player.Speed.Y - power);
            }
            else if (player.Speed.X > 0 && player.Speed.Y < 0) // Bottom Left
            {
                player.Speed = new SFML.System.Vector2f(player.Speed.X - power, player.Speed.Y + power);
            }
            else if (player.Speed.X < 0 && player.Speed.Y < 0) // Bottom Right
            {
                player.Speed = new SFML.System.Vector2f(player.Speed.X + power, player.Speed.Y + power);
            }
        }
    }
}
