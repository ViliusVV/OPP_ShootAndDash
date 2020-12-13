using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI.Interpreter
{
    class HealExpression : AbstractActionExpression
    {
        public override string ActionName() { return "heal"; }

        public override string ActionNameAlias() { return "pray to deities"; }
    }
}
