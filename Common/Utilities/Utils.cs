using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
    public static class ExtensionMethods
    {
        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
