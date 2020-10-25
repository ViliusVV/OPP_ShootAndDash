using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using SFML.Graphics;
using Client.Config;
using Client.Utilities;
using SFML.System;
using Client.Objects.Abstract_Facotry.Destructibles.Bridge;

namespace Client.Objects.Destructables
{
    class HealthCrate : Destructible
    {
        public Pickupable Pickupable { get => ItemBridge.GetPickupable(); }

        public HealthCrate(IItemBridge itemBridge)
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.MedkitCrateBrown);
            this.ItemBridge = itemBridge;
        }

        public override Sprite SpawnObject()
        {
            return this;
        }

    }
}
