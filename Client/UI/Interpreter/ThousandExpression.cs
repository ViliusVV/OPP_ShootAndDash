using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI.Interpreter
{
    class ThousandExpression : AbstractNumberExpression
    {
        public override string One() { return "M"; }
        public override string Four() { return " "; }
        public override string Five() { return " "; }
        public override string Nine() { return " "; }
        public override int Multiplier() { return 1000; }
    }
}
