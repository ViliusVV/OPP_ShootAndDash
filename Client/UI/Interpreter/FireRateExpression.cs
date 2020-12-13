using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI.Interpreter
{
    class FireRateExpression : AbstractActionExpression
    {
        public override string ActionName() { return "speed up"; }

        public override string ActionNameAlias() { return "overdrive"; }
    }
}
