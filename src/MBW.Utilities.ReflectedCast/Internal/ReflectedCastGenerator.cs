﻿using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using MBW.Utilities.ReflectedCast.Exceptions;

namespace MBW.Utilities.ReflectedCast.Internal
{
    internal static class ReflectedCastGenerator
    {
        public static TypeMap CreateWrapperType(ReflectedCasterOptions options, ModuleBuilder moduleBuilder, Type sourceType, Type targetType)
        {
            string newTypeName = "Wrapper_" + sourceType.FullName + "_" + targetType.FullName;

            TypeMap result = new TypeMap(sourceType, targetType);

            // Create a new type builder
            TypeBuilder typeBuilder = moduleBuilder.DefineType(newTypeName, TypeAttributes.Public | TypeAttributes.Class);
            typeBuilder.AddInterfaceImplementation(targetType);

            // Add constructor
            FieldBuilder internalField = typeBuilder.DefineField("_internal", sourceType, FieldAttributes.Private | FieldAttributes.InitOnly);
            ConstructorBuilder ctor = typeBuilder.DefineConstructor(
                MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.HideBySig |
                MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis, new[] { sourceType });

            {
                // Create constructor for wrapper type
                ILGenerator ctorIl = ctor.GetILGenerator();
                ConstructorInfo objectCtor = typeof(object).GetConstructor(new Type[0]);

                // Call object() base
                ctorIl.Emit(OpCodes.Ldarg_0);
                ctorIl.Emit(OpCodes.Call, objectCtor);
                ctorIl.Emit(OpCodes.Nop);

                // Set "_internal" field
                ctorIl.Emit(OpCodes.Ldarg_0);   // Load this
                ctorIl.Emit(OpCodes.Ldarg_1);   // Load argument
                ctorIl.Emit(OpCodes.Stfld, internalField);

                ctorIl.Emit(OpCodes.Ret);
            }

            // Methods (also covers properties and events)
            foreach (MethodInfo method in targetType.GetMethods())
            {
                // Find matching source method
                MethodInfo sourceMethod = FindSourceMethod(options, sourceType, targetType, method.Name, method.GetParameters().Select(s => s.ParameterType).ToArray(), method.ReturnType);

                // Generate new method
                MethodAttributes attributes = method.Attributes;
                Type[] parameterTypes = method.GetParameters().Select(s => s.ParameterType).ToArray();

                attributes &= ~MethodAttributes.Abstract;

                MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Name, attributes, method.ReturnType, parameterTypes);
                ILGenerator methodIl = methodBuilder.GetILGenerator();

                if (sourceMethod == null)
                {
                    result.MissingMethods.Add(method);

                    // Generate method that throws exception
                    // Call new ReflectedCastNotImplementedInSourceException(type, string)
                    methodIl.Emit(OpCodes.Ldtoken, sourceType);
                    
                    // sourceType is actually a RuntimeTypeHandle, so we need to convert it to a Type at runtime.
                    MethodInfo getTypeFromHandle = typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle));
                    methodIl.Emit(OpCodes.Call, getTypeFromHandle);
                    
                    methodIl.Emit(OpCodes.Ldstr, method.ToString());
                    methodIl.Emit(OpCodes.Newobj, typeof(ReflectedCastNotImplementedInSourceException).GetConstructor(new[] { typeof(Type), typeof(string) }));
                    methodIl.Emit(OpCodes.Throw);
                }
                else
                {
                    // Generate method that wraps source type
                    // Load internal field
                    methodIl.Emit(OpCodes.Ldarg_0);
                    methodIl.Emit(OpCodes.Ldfld, internalField);

                    // Prepare arguments
                    for (int i = 1; i <= method.GetParameters().Length; i++)
                    {
                        if (i == 1)
                            methodIl.Emit(OpCodes.Ldarg_1);
                        else if (i == 2)
                            methodIl.Emit(OpCodes.Ldarg_2);
                        else if (i == 3)
                            methodIl.Emit(OpCodes.Ldarg_3);
                        else
                            methodIl.Emit(OpCodes.Ldarg, i);
                    }

                    // Call sourceType method
                    methodIl.Emit(OpCodes.Callvirt, sourceMethod);

                    methodIl.Emit(OpCodes.Ret);
                }
            }

            // Generate (and deliver) wrapping type
            result.WrappingType = typeBuilder.CreateType();

            return result;
        }

        private static MethodInfo FindSourceMethod(ReflectedCasterOptions options, Type sourceType, Type targetType, string name, Type[] parameters, Type returnType)
        {
            MethodInfo method;

            bool IsValidMethod(MethodInfo candidate)
            {
                if (candidate == null)
                    return false;

                if (options.SupportSpecificReturnTypes)
                {
                    if (!returnType.IsAssignableFrom(method.ReturnType))
                        return false;
                }
                else if (method.ReturnType != returnType)
                    return false;

                return true;
            }

            if (options.SupportExplicitImplementations)
            {
                // Try explicit implementation on Interface by FullName
                foreach (Type @interface in sourceType.GetInterfaces())
                {
                    if (@interface.FullName != targetType.FullName)
                        continue;

                    method = @interface.GetMethod(name, BindingFlags.Public | BindingFlags.Instance, null, parameters, null);
                    if (IsValidMethod(method))
                        return method;
                }
            }

            if (options.SupportExplicitImplementationsByInterfaceName)
            {
                // Try explicit implementation on Interface by Name
                foreach (Type @interface in sourceType.GetInterfaces())
                {
                    if (@interface.Name != targetType.Name)
                        continue;

                    method = @interface.GetMethod(name, BindingFlags.Public | BindingFlags.Instance, null, parameters, null);
                    if (IsValidMethod(method))
                        return method;
                }
            }

            // Try implicit implementation
            method = sourceType.GetMethod(name, BindingFlags.Public | BindingFlags.Instance, null, parameters, null);
            if (IsValidMethod(method))
                return method;

            return null;
        }
    }
}