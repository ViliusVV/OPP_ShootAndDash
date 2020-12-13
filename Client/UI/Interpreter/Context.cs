using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI.Interpreter
{
    class Context
    {
        public string Input { get; set; }
        public int OutputNumber { get; set; }
        public string ActionName { get; set; }


        public Context(string input)
        {
            this.Input = input;
        }
    }
}
