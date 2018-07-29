using Xunit;

namespace ReflectedCast.Tests
{
    public class ExplicitInheritedImplementationCases
    {
        [Fact]
        public void ExplicitInheritedImplementation()
        {
            TestClass orig = new TestClass();
            ITestInterfaceFirst asInterfaceFirst = ReflectedCaster.Default.CastToInterface<ITestInterfaceFirst>(orig);
            ITestInterfaceSecond asInterfaceSecond = ReflectedCaster.Default.CastToInterface<ITestInterfaceSecond>(orig);

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

        public class TestBaseClass : ITestInterfaceFirst
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
        }

        public class TestClass : TestBaseClass, ITestInterfaceSecond
        {
            void ITestInterfaceSecond.Method()
            {
                ReturnCode = 2;
            }
        }
    }
}