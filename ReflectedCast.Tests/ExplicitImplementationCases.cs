using Xunit;

namespace ReflectedCast.Tests
{
    public class ExplicitImplementationCases
    {
        [Fact]
        public void ExplicitImplementation()
        {
            TestClass orig = new TestClass();
            ITestInterfaceFirst asInterfaceFirst = ReflectedCaster.CastToInterface<ITestInterfaceFirst>(orig);
            ITestInterfaceSecond asInterfaceSecond = ReflectedCaster.CastToInterface<ITestInterfaceSecond>(orig);

            asInterfaceFirst.Method();
            Assert.Equal(1, orig.ReturnCode);

            asInterfaceSecond.Method();
            Assert.Equal(2, orig.ReturnCode);
        }

        public interface ITestInterfaceFirst
        {
            void Method();
        }

        public interface ITestInterfaceSecond
        {
            void Method();
        }

        public class TestClass : ITestInterfaceFirst, ITestInterfaceSecond
        {
            public object ReturnCode { get; set; }

            public void Method()
            {
                ReturnCode = 3;
            }

            void ITestInterfaceFirst.Method()
            {
                ReturnCode = 1;
            }

            void ITestInterfaceSecond.Method()
            {
                ReturnCode = 2;
            }
        }
    }
}