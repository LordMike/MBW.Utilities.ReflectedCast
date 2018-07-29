using ReflectedCast.Exceptions;
using Xunit;

namespace ReflectedCast.Tests
{
    public class ErrorCases
    {
        [Fact]
        public void MissingFunction()
        {
            TestClass orig = new TestClass();

            Assert.Throws<ReflectedCastMissingMethodsException>(() => ReflectedCaster.CastToInterface<ITestInterface>(orig));
        }

        [Fact]
        public void MissingFunctionAllow()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.CastToInterface<ITestInterface>(orig, CastOptions.AllowMissingFunctions);

            Assert.Throws<ReflectedCastNotImplementedInSourceException>(() => asInterface.Method());
        }

        public interface ITestInterface
        {
            void Method();
        }

        public class TestClass
        {
        }
    }
}