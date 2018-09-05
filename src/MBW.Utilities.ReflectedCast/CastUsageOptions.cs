using System;

namespace MBW.Utilities.ReflectedCast
{
    [Flags]
    public enum CastUsageOptions
    {
        None = 0,
        AllowMissingFunctions = 1
    }
}