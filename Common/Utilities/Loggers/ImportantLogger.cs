using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Utilities.Loggers
{
    class ImportantLogger : AbstractLogger
    {
        public ImportantLogger(int level)
        {
            logableMessageLevel = level;
        }


        protected override void write(int level, string message, string file, string member, int line)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("|{4}| [ {0}#{1}({2}) ] {3}", Path.GetFileName(file), member, line, message, level);
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}
