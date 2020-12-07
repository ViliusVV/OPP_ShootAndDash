using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Common.Utilities.Loggers
{
    abstract class AbstractLogger
    {
        protected AbstractLogger next;
        protected int logableMessageLevel;
        public void SetNext(AbstractLogger nextLogger)
        {
            next = nextLogger;
        }
        public void LogMessage(int level, string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            if(logableMessageLevel <= level)
            {
                write(level, message, file, member, line);
            }
            if(next != null)
            {
                next.LogMessage(level, message, file, member, line);
            }

        }
        abstract protected void write(int level, string message, string file, string member, int line);
    }
}
