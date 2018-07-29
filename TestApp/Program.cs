using System;
using ReflectedCast;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClass orig = new TestClass();
            ITest asInterface = ReflectedCaster.CastToInterface<ITest>(orig);

            asInterface.MyProperty = "SomeValue";
            asInterface.Event += () => Console.WriteLine("Event called");
            asInterface.HelloWorld("Test", 4);
            Console.WriteLine(asInterface.Add(4, 5));
        }
    }

    public interface ITest
    {
        event Action Event;

        string MyProperty { get; set; }

        void HelloWorld(string a, int b);

        int Add(int a, int b);
    }

    public class TestClass
    {
        public event Action Event;

        public string MyProperty { get; set; }

        public void HelloWorld(string a, int b)
        {
            Event?.Invoke();
            Console.WriteLine("Ohai; Prop: " + MyProperty + ";; " + a + ", " + b);
        }

        public int Add(int a, int b)
        {
            return a + b;
        }
    }




    class TestWrapper : ITest
    {
        private readonly TestClass _clss;

        public TestWrapper(TestClass clss)
        {
            _clss = clss;
        }

        public event Action Event;

        public string MyProperty
        {
            get => throw new NotImplementedException("The wrapped type");
            set => throw new NotImplementedException();
        }

        public void HelloWorld(string a, int b)
        {
            _clss.HelloWorld(a, b);
        }

        public int Add(int a, int b)
        {
            return _clss.Add(a, b);
        }
    }
}
