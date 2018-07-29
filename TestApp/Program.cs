using System;
using ReflectedCast;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClass orig = new TestClass();
            ITestInterfaceFirst asInterfaceFirst = ReflectedCaster.CastToInterface<ITestInterfaceFirst>(orig);
            ITestInterfaceSecond asInterfaceSecond = ReflectedCaster.CastToInterface<ITestInterfaceSecond>(orig);

            asInterfaceFirst.Method();
            asInterfaceSecond.Method();
        }
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
