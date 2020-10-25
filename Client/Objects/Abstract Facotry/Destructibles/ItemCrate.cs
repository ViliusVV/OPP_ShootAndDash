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
    class ItemCrate : Destructible
    {
        public Pickupable Pickupable { get => ItemBridge.GetPickupable(); }

        public ItemCrate(IItemBridge itemBridge)
        {
            this.Texture = TextureHolder.GetInstance().Get(TextureIdentifier.CrateBrown);
            this.ItemBridge = itemBridge;
        }

        public override Sprite SpawnObject()
        {
            return this;
        }
    }
}
