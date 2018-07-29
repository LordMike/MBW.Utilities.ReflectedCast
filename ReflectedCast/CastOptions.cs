using System;

namespace ReflectedCast
{
    [Flags]
    public enum CastOptions
    {
        None = 0,
        AllowMissingFunctions = 1
    }
}