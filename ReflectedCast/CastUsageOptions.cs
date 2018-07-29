using System;

namespace ReflectedCast
{
    [Flags]
    public enum CastUsageOptions
    {
        None = 0,
        AllowMissingFunctions = 1
    }
}