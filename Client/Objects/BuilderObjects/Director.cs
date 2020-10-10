using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.BuilderObjects
{
    class Director
    {
        IBuilder Builder;
        public Director(IBuilder builder)
        {
            Builder = builder;
        }
        public void Construct()
        {
            Builder.Reset().StartNew(64, 48).BuildWalls().BuildBuilding();
        }
        public void ConstructBase()
        {
            Builder.Reset().StartNew(32, 32);
        }
    }
}
