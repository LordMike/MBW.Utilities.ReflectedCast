using System;
using ReflectedCast;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.Default.CastToInterface<ITestInterface>(orig);

            asInterface.Method();

            Console.WriteLine(orig.ReturnCode);
        }
    }

    public interface ITestInterface
    {
        void Method();
    }

    public class TestClass
    {
        public object ReturnCode { get; set; }

        public void Method()
        {
            ReturnCode = "Hello world";
        }
    }
}
