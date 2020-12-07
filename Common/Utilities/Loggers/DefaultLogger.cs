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

        public override void LogMessage(int level, string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            if (logableMessageLevel <= level && level < 20)
            {
                write(level, message, file, member, line);
            }
            if (next != null)
            {
                next.LogMessage(level, message, file, member, line);
            }

        }
        protected override void write(int level, string message, string file, string member, int line)
        {
            Console.WriteLine("|{4}| [{0}#{1}({2})] {3}", Path.GetFileName(file), member, line, message, level);
        }
    }
}
