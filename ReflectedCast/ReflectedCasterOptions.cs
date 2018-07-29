using JetBrains.Annotations;

namespace ReflectedCast
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

        internal ReflectedCasterOptions Clone()
        {
            return new ReflectedCasterOptions
            {
                SupportExplicitImplementations = SupportExplicitImplementations,
                SupportExplicitImplementationsByInterfaceName = SupportExplicitImplementationsByInterfaceName
            };
        }
    }
}