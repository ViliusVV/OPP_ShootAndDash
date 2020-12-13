using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI.Interpreter
{
    abstract class AbstractActionExpression : IExpression
    {
        public void Interpret(Context context)
        {
            if (context.Input.Length == 0)
                return;
            if (context.Input.StartsWith(ActionName()))
            {
                context.ActionName = ActionName();
                context.Input = context.Input.Substring(ActionName().Length).Trim();
            }
            else if (context.Input.StartsWith(ActionNameAlias()))
            {
                context.ActionName = ActionName();
                context.Input = context.Input.Substring(ActionNameAlias().Length).Trim();
            }
        }

        public abstract string ActionName();
        public abstract string ActionNameAlias();
    }
}
