using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Client.Config
{
    // Custom resource atribute to implement enums with custom values
    class ResourceAttr : Attribute
    {
        public string Path { get; private set; }
        public bool IgnoreBatchLoad { get; set; }


        public ResourceAttr(string path)
        {
            this.Path = path;
        }

        public ResourceAttr(string path, bool ignoreBatchLoad): this(path)
        {
            this.IgnoreBatchLoad = ignoreBatchLoad;
        }
    }

    // Game resrouce class which helps to pull field data form enums with ResoureAttr atribute
    public static class GameResourceHelper
    {
        private static ResourceAttr GetAttr(Object r)
        {
            return (ResourceAttr)Attribute.GetCustomAttribute(ForValue(r), typeof(ResourceAttr));
        }


        private static MemberInfo ForValue(Object r)
        {
            return r.GetType().GetField(Enum.GetName(r.GetType(), r));
        }


        public static string GetResoucePath<R>(this R r) where R: Enum
        {
            ResourceAttr attr = GetAttr(r);
            return attr.Path;
        }
    }


}
