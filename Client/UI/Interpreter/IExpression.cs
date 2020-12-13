using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI.Interpreter
{
    interface IExpression
    {
        public void Interpret(Context context);
    }
}
