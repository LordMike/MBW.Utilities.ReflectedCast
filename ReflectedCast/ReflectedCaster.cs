using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ReflectedCast.Exceptions;
using ReflectedCast.Internal;

namespace ReflectedCast
{
    public static class ReflectedCaster
    {
        private static readonly Dictionary<TypeMapKey, TypeMap> Maps;

        static ReflectedCaster()
        {
            Maps = new Dictionary<TypeMapKey, TypeMap>();
        }

        private static void CheckOptions(TypeMap wrappedResult, CastOptions options)
        {
            if (!options.HasFlag(CastOptions.AllowMissingFunctions) && wrappedResult.MissingMethods.Any())
            {
                // Must not have missing methods
                throw new ReflectedCastMissingMethodsException(wrappedResult);
            }
        }

        [PublicAPI]
        public static T CastToInterface<T>(object instance, CastOptions options = CastOptions.None)
        {
            Type targetType = typeof(T);
            if (!targetType.IsInterface)
                throw new ArgumentException();

            return (T)CastToInterface(instance, targetType, options);
        }

        [PublicAPI]
        public static object CastToInterface(object instance, Type targetType, CastOptions options = CastOptions.None)
        {
            if (!targetType.IsInterface)
                throw new ArgumentException();

            Type sourceType = instance.GetType();

            if (!Maps.TryGetValue(new TypeMapKey(sourceType, targetType), out TypeMap wrappedResult))
                Maps[new TypeMapKey(sourceType, targetType)] = wrappedResult = ReflectedCastGenerator.CreateWrapperType(sourceType, targetType);

            // Check options
            CheckOptions(wrappedResult, options);

            // Return result
            return Activator.CreateInstance(wrappedResult.WrappingType, instance);
        }
    }
}