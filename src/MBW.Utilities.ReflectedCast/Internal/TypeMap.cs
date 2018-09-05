using System;
using System.Collections.Generic;
using System.Reflection;

namespace MBW.Utilities.ReflectedCast.Internal
{
    internal class TypeMap
    {
        public Type SourceType { get; }
        public Type TargetType { get; }

        public Type WrappingType { get; set; }

        public List<MethodInfo> MissingMethods { get; }

        public TypeMap(Type sourceType, Type targetType)
        {
            SourceType = sourceType;
            TargetType = targetType;
            MissingMethods = new List<MethodInfo>();
        }
    }
}