using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI.Interpreter
{
    class TenExpression : AbstractNumberExpression
    {
        public override string One() { return "X"; }
        public override string Four() { return "XL"; }
        public override string Five() { return "L"; }
        public override string Nine() { return "XC"; }
        public override int Multiplier() { return 10; }
    }
}
