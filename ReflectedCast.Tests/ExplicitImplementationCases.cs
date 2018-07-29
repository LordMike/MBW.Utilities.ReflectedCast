using Xunit;

namespace ReflectedCast.Tests
{
    public class ExplicitImplementationCases
    {
        [Fact]
        public void ExplicitImplementation()
        {
            // Tests a straightforward setup (same interfaces)
            TestClass orig = new TestClass();
            FirstScope.ITestInterfaceFirst asInterfaceFirst = ReflectedCaster.CastToInterface<FirstScope.ITestInterfaceFirst>(orig);
            FirstScope.ITestInterfaceSecond asInterfaceSecond = ReflectedCaster.CastToInterface<FirstScope.ITestInterfaceSecond>(orig);

            asInterfaceFirst.Method();
            Assert.Equal(1, orig.ReturnCode);

            asInterfaceSecond.Method();
            Assert.Equal(2, orig.ReturnCode);
        }

        [Fact]
        public void ExplicitImplementationDifferentInterfaces()
        {
            // Tests the case where the interfaces are actually different, but the names are the same
            TestClass orig = new TestClass();
            OtherScope.ITestInterfaceFirst asInterfaceFirst = ReflectedCaster.CastToInterface<OtherScope.ITestInterfaceFirst>(orig);
            OtherScope.ITestInterfaceSecond asInterfaceSecond = ReflectedCaster.CastToInterface< OtherScope.ITestInterfaceSecond> (orig);

            asInterfaceFirst.Method();
            Assert.Equal(1, orig.ReturnCode);

            asInterfaceSecond.Method();
            Assert.Equal(2, orig.ReturnCode);
        }

        public class TestClass : FirstScope.ITestInterfaceFirst, FirstScope.ITestInterfaceSecond
        {
            public object ReturnCode { get; set; }

            public void Method()
            {
                ReturnCode = 3;
            }

            void FirstScope.ITestInterfaceFirst.Method()
            {
                ReturnCode = 1;
            }

            void FirstScope.ITestInterfaceSecond.Method()
            {
                ReturnCode = 2;
            }
        }

        public class FirstScope
        {
            public interface ITestInterfaceFirst
            {
                void Method();
            }

            public interface ITestInterfaceSecond
            {
                void Method();
            }
        }

        public class OtherScope
        {
            public interface ITestInterfaceFirst
            {
                void Method();
            }

            public interface ITestInterfaceSecond
            {
                void Method();
            }
        }
    }
}