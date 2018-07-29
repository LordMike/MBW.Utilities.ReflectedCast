using System;
using Xunit;

namespace ReflectedCast.Tests
{
    public class Basics
    {
        [Fact]
        public void Event()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.CastToInterface<ITestInterface>(orig);

            int counter = 0;
            asInterface.Event += () => counter++;
            asInterface.Method();

            Assert.Equal(1, orig.ReturnCode);
            Assert.Equal(1, counter);
        }

        [Fact]
        public void Property()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.CastToInterface<ITestInterface>(orig);

            asInterface.Property = "Hello world";

            Assert.Equal("Hello world", orig.Property);
            Assert.Equal("Hello world", asInterface.Property);
        }

        [Fact]
        public void PropertyGetOnly()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.CastToInterface<ITestInterface>(orig);

            orig.PropertyGetOnly = "Hello world";

            Assert.Equal("Hello world", asInterface.PropertyGetOnly);
        }

        [Fact]
        public void PropertySetOnly()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.CastToInterface<ITestInterface>(orig);

            asInterface.PropertySetOnly = "Hello world";

            Assert.Equal("Hello world", orig.PropertySetOnly);
        }

        [Fact]
        public void Method()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.CastToInterface<ITestInterface>(orig);

            asInterface.Method();

            Assert.Equal(1, orig.ReturnCode);
        }

        [Fact]
        public void MethodOneArg()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.CastToInterface<ITestInterface>(orig);

            asInterface.MethodOneArg("Hello world");

            Assert.Equal("Hello world", orig.ReturnCode);
        }

        [Fact]
        public void MethodTwoArg()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.CastToInterface<ITestInterface>(orig);

            asInterface.MethodTwoArg("Hello world", 18);

            Assert.Equal($"Hello world{18}", orig.ReturnCode);
        }

        [Fact]
        public void MethodTenArg()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.CastToInterface<ITestInterface>(orig);

            asInterface.MethodTenArg("Hello world", 1, "c", 2, "e", 3, "g", 4, "i", 5);

            Assert.Equal($"Hello world{1}c{2}e{3}g{4}i{5}", orig.ReturnCode);
        }

        [Fact]
        public void MethodReturnOneArg()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.CastToInterface<ITestInterface>(orig);

            string ret = asInterface.MethodReturnOneArg("Hello world");

            Assert.Equal("Hello world", orig.ReturnCode);
            Assert.Equal("Hello world", ret);
        }

        [Fact]
        public void MethodReturnTwoArg()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.CastToInterface<ITestInterface>(orig);

            string ret = asInterface.MethodReturnTwoArg("Hello world", 18);

            Assert.Equal("Hello world18", orig.ReturnCode);
            Assert.Equal("Hello world18", ret);
        }

        [Fact]
        public void MethodReturnTenArg()
        {
            TestClass orig = new TestClass();
            ITestInterface asInterface = ReflectedCaster.CastToInterface<ITestInterface>(orig);

            string ret = asInterface.MethodReturnTenArg("Hello world", 1, "c", 2, "e", 3, "g", 4, "i", 5);

            Assert.Equal($"Hello world{1}c{2}e{3}g{4}i{5}", orig.ReturnCode);
            Assert.Equal($"Hello world{1}c{2}e{3}g{4}i{5}", ret);
        }

        public interface ITestInterface
        {
            event Action Event;

            string Property { get; set; }

            string PropertyGetOnly { get; }

            string PropertySetOnly { set; }

            void Method();

            void MethodOneArg(string a);

            void MethodTwoArg(string a, int b);

            void MethodTenArg(string a, int b, string c, int d, string e, int f, string g, int h, string i, int j);

            string MethodReturnOneArg(string a);

            string MethodReturnTwoArg(string a, int b);

            string MethodReturnTenArg(string a, int b, string c, int d, string e, int f, string g, int h, string i, int j);
        }

        public class TestClass
        {
            public object ReturnCode { get; private set; }

            public event Action Event;

            public string Property { get; set; }

            public string PropertyGetOnly { get; set; }

            public string PropertySetOnly { get; set; }

            public void Method()
            {
                ReturnCode = 1;
                Event?.Invoke();
            }

            public void MethodOneArg(string a)
            {
                ReturnCode = a;
            }

            public void MethodTwoArg(string a, int b)
            {
                ReturnCode = a + b;
            }

            public void MethodTenArg(string a, int b, string c, int d, string e, int f, string g, int h, string i, int j)
            {
                ReturnCode = a + b + c + d + e + f + g + h + i + j;
            }

            public string MethodReturnOneArg(string a)
            {
                ReturnCode = a;
                return a;
            }

            public string MethodReturnTwoArg(string a, int b)
            {
                string val = a + b;
                ReturnCode = val;
                return val;
            }

            public string MethodReturnTenArg(string a, int b, string c, int d, string e, int f, string g, int h, string i, int j)
            {
                string val = a + b + c + d + e + f + g + h + i + j;
                ReturnCode = val;
                return val;
            }
        }
    }
}