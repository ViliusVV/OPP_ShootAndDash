using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Common.Utilities.Loggers
{
    class DefaultLogger : AbstractLogger
    {
        public DefaultLogger(int level)
        {
            logableMessageLevel = level;
        }

        public static void Log(int level, string text, string file, string member, int line)
        {
            Console.WriteLine("|{4}| [ {0}#{1}({2}) ] {3}", Path.GetFileName(file), member, line, text, level);
        }

        protected override void write(int level, string message, string file, string member, int line)
        {
            Log(level, message, file, member, line);
        }
    }
}
