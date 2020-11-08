using System;
using System.Collections.Generic;
using System.Text;
using Client.Managers;
using Client.Objects.Abstract_Facotry.Destructibles.Bridge;
using Client.Objects.Destructables;
using Client.Objects.Indestructables;

namespace Client.Objects
{
    class DestructiblesFactory : AbstractFactory
    {
        public override Indestructible GetIndestructible(string indestructibleObj)
        {
            return null;
        }
        public override Destructible GetDestructible(string destructibleObj)
        {
            if (destructibleObj == null)
                return null;

            if (destructibleObj.Equals("HealthCrate"))
            {
                bool randomChoice = GameState.GetInstance().Random.NextDouble() >= 0.5;
                IItemBridge ItemBridge;
                if (randomChoice)
                    ItemBridge = new MedkitBridge();
                else
                    ItemBridge = new HealingSyringeBridge();
                return new HealthCrate(ItemBridge);
            }
            else if (destructibleObj.Equals("ItemCrate"))
            {
                return new ItemCrate(new GunBridge());
            }
            return null;
        }
    }
}
