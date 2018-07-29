using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ReflectedCast.Exceptions;
using ReflectedCast.Internal;

namespace ReflectedCast
{
    public class ReflectedCaster
    {
        public static ReflectedCaster Default { get; } = new ReflectedCaster();

        private readonly Dictionary<TypeMapKey, TypeMap> _maps;
        private readonly ReflectedCasterOptions _options;

        public ReflectedCaster(ReflectedCasterOptions options = null)
        {
            _options = options == null ? new ReflectedCasterOptions() : options.Clone();

            _maps = new Dictionary<TypeMapKey, TypeMap>();
        }

        private static void CheckOptions(TypeMap wrappedResult, CastUsageOptions usageOptions)
        {
            if (!usageOptions.HasFlag(CastUsageOptions.AllowMissingFunctions) && wrappedResult.MissingMethods.Any())
            {
                // Must not have missing methods
                throw new ReflectedCastMissingMethodsException(wrappedResult);
            }
        }

        [PublicAPI]
        public T CastToInterface<T>(object instance, CastUsageOptions usageOptions = CastUsageOptions.None)
        {
            Type targetType = typeof(T);
            if (!targetType.IsInterface)
                throw new ArgumentException();

            return (T)CastToInterface(instance, targetType, usageOptions);
        }

        [PublicAPI]
        public object CastToInterface(object instance, Type targetType, CastUsageOptions usageOptions = CastUsageOptions.None)
        {
            if (!targetType.IsInterface)
                throw new ArgumentException();

            Type sourceType = instance.GetType();

            if (!_maps.TryGetValue(new TypeMapKey(sourceType, targetType), out TypeMap wrappedResult))
                _maps[new TypeMapKey(sourceType, targetType)] = wrappedResult = ReflectedCastGenerator.CreateWrapperType(_options, sourceType, targetType);

            // Check options
            CheckOptions(wrappedResult, usageOptions);

            // Return result
            return Activator.CreateInstance(wrappedResult.WrappingType, instance);
        }
    }
}