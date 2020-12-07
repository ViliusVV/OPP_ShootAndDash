using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Utilities.Loggers
{
    class CriticalLogger : AbstractLogger
    {
        public CriticalLogger(int level)
        {
            logableMessageLevel = level;
        }

        protected override void write(int level, string message, string file, string member, int line)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("|{4}| {5} [ {0}#{1}({2}) ] {3}", Path.GetFileName(file), member, line, message, level, DateTime.Now);
            Console.ForegroundColor = ConsoleColor.White;
            using (StreamWriter writer = File.AppendText("critical_logs.txt"))
            {
                writer.WriteLine("|{4}| {5} [ {0}#{1}({2}) ] {3}", Path.GetFileName(file), member, line, message, level, DateTime.Now);
            }
        }
    }
}
