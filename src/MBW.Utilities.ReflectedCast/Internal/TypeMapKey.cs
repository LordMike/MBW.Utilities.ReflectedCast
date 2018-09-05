using System;

namespace MBW.Utilities.ReflectedCast.Internal
{
    internal struct TypeMapKey : IEquatable<TypeMapKey>
    {
        private readonly Type _a;
        private readonly Type _b;

        public TypeMapKey(Type a, Type b)
        {
            _a = a;
            _b = b;
        }

        public bool Equals(TypeMapKey other)
        {
            return _a == other._a && _b == other._b;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is TypeMapKey key && Equals(key);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_a.GetHashCode() * 397) ^ _b.GetHashCode();
            }
        }
    }
}
