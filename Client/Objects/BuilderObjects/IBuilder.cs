using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.BuilderObjects
{
    interface IBuilder
    {
        public IBuilder StartNew(int length, int width);
        public IBuilder BuildWalls();
        public IBuilder BuildBuilding();
        public IBuilder Reset();

    }
}
