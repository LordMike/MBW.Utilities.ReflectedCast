using System;

namespace MBW.Utilities.ReflectedCast.Exceptions
{
    public class ReflectedCastNotImplementedInSourceException : Exception
    {
        public ReflectedCastNotImplementedInSourceException(Type sourceType, string method) : base(Format(sourceType, method))
        {
        }

        private static string Format(Type sourceType, string method)
        {
            return $"The source type {sourceType.FullName} is missing {method}";
        }
    }
}