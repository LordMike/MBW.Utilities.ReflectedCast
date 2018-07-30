using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using JetBrains.Annotations;
using ReflectedCast.Exceptions;
using ReflectedCast.Internal;

namespace ReflectedCast
{
    public class ReflectedCaster
    {
        public static ReflectedCaster Default { get; } = new ReflectedCaster();

        private readonly ConcurrentDictionary<TypeMapKey, TypeMap> _maps;
        private readonly ReflectedCasterOptions _options;
        private readonly ModuleBuilder _moduleBuilder;

        public ReflectedCaster(ReflectedCasterOptions options = null)
        {
            _options = options == null ? new ReflectedCasterOptions() : options.Clone();
            _maps = new ConcurrentDictionary<TypeMapKey, TypeMap>();

            // Create a dynamic assembly and module 
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("ProxyAssembly-" + Guid.NewGuid()), AssemblyBuilderAccess.Run);
            _moduleBuilder = assemblyBuilder.DefineDynamicModule("ProxyModule");
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

            TypeMap wrappedResult = _maps.GetOrAdd(new TypeMapKey(sourceType, targetType), key => ReflectedCastGenerator.CreateWrapperType(_options, _moduleBuilder, sourceType, targetType));

            // Check options
            CheckOptions(wrappedResult, usageOptions);

            // Return result
            return Activator.CreateInstance(wrappedResult.WrappingType, instance);
        }
    }
}