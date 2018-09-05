using JetBrains.Annotations;

namespace MBW.Utilities.ReflectedCast
{
    [PublicAPI]
    public class ReflectedCasterOptions
    {
        public ReflectedCasterOptions()
        {
            SupportExplicitImplementations = true;
            SupportExplicitImplementationsByInterfaceName = true;
        }

        /// <summary>
        /// Supports explicit implementations, like:
        /// void MyInterface.MyMethod();
        /// </summary>
        public bool SupportExplicitImplementations { get; set; }

        public ReflectedCasterOptions SetSupportExplicitImplementations(bool supported = true)
        {
            SupportExplicitImplementations = supported;
            return this;
        }

        /// <summary>
        /// Supports explicit implementations, like:
        /// void MyInterface.MyMethod();
        ///
        /// Unlike <see cref="SupportExplicitImplementations"/>, this will only check for implementations interfaces' by Name (not FullName)
        /// </summary>
        public bool SupportExplicitImplementationsByInterfaceName { get; set; }

        public ReflectedCasterOptions SetSupportExplicitImplementationsByInterfaceName(bool supported = true)
        {
            SupportExplicitImplementationsByInterfaceName = supported;
            return this;
        }

        /// <summary>
        /// Allows finding mapping to methods that return more specific types, such as:
        /// Instance:
        ///   MyClass MyMethod();
        ///
        /// Interface:
        ///   MyBaseClass MyMethod();
        /// </summary>
        public bool SupportSpecificReturnTypes { get; set; }

        public ReflectedCasterOptions SetSupportSpecificReturnTypes(bool supported = true)
        {
            SupportSpecificReturnTypes = supported;
            return this;
        }

        internal ReflectedCasterOptions Clone()
        {
            return new ReflectedCasterOptions
            {
                SupportExplicitImplementations = SupportExplicitImplementations,
                SupportExplicitImplementationsByInterfaceName = SupportExplicitImplementationsByInterfaceName,
                SupportSpecificReturnTypes = SupportSpecificReturnTypes
            };
        }
    }
}