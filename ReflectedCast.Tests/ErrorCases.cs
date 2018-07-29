using ReflectedCast.Exceptions;
using Xunit;

namespace ReflectedCast.Tests
{
    public class ErrorCases
    {
        [Fact]
        public void MissingFunction()
        {
            TestClass1 orig = new TestClass1();

            Assert.Throws<ReflectedCastMissingMethodsException>(() => ReflectedCaster.CastToInterface<ITestInterface1>(orig));
        }

        [Fact]
        public void MissingFunctionAllow()
        {
            TestClass1 orig = new TestClass1();
            ITestInterface1 asInterface = ReflectedCaster.CastToInterface<ITestInterface1>(orig, CastOptions.AllowMissingFunctions);

            Assert.Throws<ReflectedCastNotImplementedInSourceException>(() => asInterface.Method());
        }

        [Fact]
        public void WrongReturn()
        {
            TestClass2 orig = new TestClass2();

            Assert.Throws<ReflectedCastMissingMethodsException>(() => ReflectedCaster.CastToInterface<ITestInterface2>(orig));
        }

        [Fact]
        public void WrongReturnAllow()
        {
            TestClass2 orig = new TestClass2();
            ITestInterface2 asInterface = ReflectedCaster.CastToInterface<ITestInterface2>(orig, CastOptions.AllowMissingFunctions);

            Assert.Throws<ReflectedCastNotImplementedInSourceException>(() => asInterface.MethodReturn());
        }

        public interface ITestInterface1
        {
            void Method();
        }

        public class TestClass1
        {
        }

        public interface ITestInterface2
        {
            int MethodReturn();
        }

        public class TestClass2
        {
            public string MethodReturn()
            {
                return "Hello world";
            }
        }
    }
}