using Client.Config;
using Client.Managers;
using Client.Models;
using Client.Utilities;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Memento
{
    class PortalPickupCheck : Pickupable
    {
        public PortalPickupCheck()
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.Portal);
        }

        public override void Pickup(Player player)
        {
            this.PickedUp = true;
            //if (!GameState.GetInstance().PortalPickedUp)
            //{
            //    GameState.GetInstance().PortalPickedUp = true;
            //    player.Position = new Vector2f(2000f, 2000f);
            //    this.Position = new Vector2f(400f, 400f);
            //}
            //else
            //{
            //    GameState.GetInstance().PortalPickedUp = false;
            //    player.Position = new Vector2f(2000f, 2000f);
            //    this.Position = new Vector2f(200f, 200f);
            //}
            
            //Caretaker caretaker = new Caretaker();
            //caretaker.Memento = portal.CreateMemento();

            //portal.Pos = new Vector2f(400f, 400f);
        }
    }
}
