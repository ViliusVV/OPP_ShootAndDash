using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utilities
{
    class Utils
    {
        public static Random Rng = new Random();

        public static T RandomEnum<T>()
        {
            Type type = typeof(T);
            Array values = Enum.GetValues(type);
            lock (Rng)
            {
                object value = values.GetValue(Rng.Next(values.Length));
                return (T)Convert.ChangeType(value, type);
            }
        }
    }
}
