using Xunit;

namespace MBW.Utilities.ReflectedCast.Tests
{
    public class InheritedCases
    {
        [Fact]
        public void InheritedMethod()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.Default.CastToInterface<ITestInterface>(orig);

            asInterface.Method();
            Assert.Equal(1, orig.ReturnCode);
        }

        public interface ITestInterface
        {
            void Method();
        }

        public class TestBaseClass
        {
            public object ReturnCode { get; set; }

            public void Method()
            {
                ReturnCode = 1;
            }
        }

        public class TestClass : TestBaseClass
        {

        }
    }
}