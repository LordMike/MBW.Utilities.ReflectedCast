using MBW.Utilities.ReflectedCast.Exceptions;
using Xunit;

namespace MBW.Utilities.ReflectedCast.Tests
{
    public class SpecificReturnTypeCases
    {
        [Fact]
        public void SpecificReturnType_NoOption()
        {
            ReflectedCaster caster = new ReflectedCaster(new ReflectedCasterOptions().SetSupportSpecificReturnTypes(false));

            TestClass orig = new TestClass();
            Assert.Throws<ReflectedCastMissingMethodsException>(() => caster.CastToInterface<ITestInterface>(orig));
        }

        [Fact]
        public void SpecificReturnType()
        {
            ReflectedCaster caster = new ReflectedCaster(new ReflectedCasterOptions().SetSupportSpecificReturnTypes());

            TestClass orig = new TestClass();
            ITestInterface asInterface = caster.CastToInterface<ITestInterface>(orig);

            object ret = asInterface.Method();
            Assert.Equal("Hello world", ret);
        }

        public interface ITestInterface
        {
            object Method();
        }

        public class TestClass
        {
            public string Method()
            {
                return "Hello world";
            }
        }
    }
}