using System;
using MBW.Utilities.ReflectedCast;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.Default.CastToInterface<ITestInterface>(orig);

            Console.WriteLine(asInterface.Method());
        }
    }

    public interface ITestInterface
    {
        string Method();
    }

    public class TestClass
    {
        public string Method()
        {
            return "Hello world";
        }
    }
}
