using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Common.Utilities.Loggers
{
    class FileLogger : AbstractLogger
    {
        string fileName;
        public FileLogger(int level, string fileName)
        {
            logableMessageLevel = level;
            this.fileName = fileName;
        }

        protected override void write(int level, string message, string file, string member, int line)
        {
            using (StreamWriter writer = File.AppendText(fileName))
            {
                writer.WriteLine("|{4}| [ {0}#{1}({2}) ] {3}", Path.GetFileName(file), member, line, message, level);

            }
        }
    }
}
