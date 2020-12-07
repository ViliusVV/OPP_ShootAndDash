using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Utilities.Loggers
{
    class ConsoleLogger : AbstractLogger
    {
        public ConsoleLogger(int level)
        {
            logableMessageLevel = level;
        }

        protected override void write(int level, string message, string file, string member, int line)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("|{4}| [ {0}#{1}({2}) ] {3}", Path.GetFileName(file), member, line, message, level);
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}
