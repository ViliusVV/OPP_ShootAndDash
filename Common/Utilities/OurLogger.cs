using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Common.Utilities
{
    class OurLogger
    {
        public static readonly OurLogger _ourLogger = new OurLogger();

        private OurLogger() { }

        public OurLogger GetInstance()
        {
            return _ourLogger;
        }

        public static void Log(string text, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            Console.WriteLine("[ {0}#{1}({2}) ] {3}", Path.GetFileName(file), member, line, text);
        }
    }
}
