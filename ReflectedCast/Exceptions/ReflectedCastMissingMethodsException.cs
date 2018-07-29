using System;
using System.Reflection;
using ReflectedCast.Internal;

namespace ReflectedCast.Exceptions
{
    public class ReflectedCastMissingMethodsException : Exception
    {
        internal ReflectedCastMissingMethodsException(TypeMap typeMap) : base(Format(typeMap))
        {
        }

        private static string Format(TypeMap typeMap)
        {
            string str = "The source type " + typeMap.SourceType.FullName + " is missing " + typeMap.MissingMethods.Count +
                         " methods before it matches " + typeMap.TargetType.FullName + ":";

            foreach (MethodInfo missingMethod in typeMap.MissingMethods)
                str += Environment.NewLine + missingMethod;

            return str;
        }
    }
}