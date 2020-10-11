using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects
{
    class FactoryProducer
    {
        public static AbstractFactory GetFactory(string choice)
        {
            if (choice.Equals("Destructible"))
            {
                return new DestructiblesFactory();
            }
            else if (choice.Equals("Indestructible"))
            {
                return new IndestructiblesFactory();
            }
            return null;
        }
    }
}
