## ReflectedCast [![Generic Build](https://github.com/LordMike/MBW.Utilities.ReflectedCast/actions/workflows/dotnet.yml/badge.svg)](https://github.com/LordMike/MBW.Utilities.ReflectedCast/actions/workflows/dotnet.yml) [![NuGet](https://img.shields.io/nuget/v/MBW.Utilities.ReflectedCast.svg)](https://www.nuget.org/packages/MBW.Utilities.ReflectedCast) [![GHPackages](https://img.shields.io/badge/package-alpha-green)](https://github.com/LordMike/MBW.Utilities.ReflectedCast/packages/692171)

A library to cast one object to an interface, from which it _doesn't_ inherit. 

### Use cases:

* You're hacking around a lot, and need to work with IL emitting code
* You're loading in assemblies with your own `AssemblyLoadContext`, you've messed up the load dependencies

## Usage

In general, you'll have an interface and an object that you wish to cast to that interface. The ReflectedCast can be used like this. Notice how the class `TestClass` does not inherit from `ITestInterface`.

```csharp
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
```

## Notes:

* You can use `ReflectedCaster.Default` or create a new `ReflectedCaster` with certain options.
* Once you have a `ReflectedCaster`, you can use `CastToInterface()` to do the casting. Here you can also specify more options for your cast
* All involved interfaces, classes and methods / properties / events .., **MUST be public**. They will be invoked from a new assembly, and need to be acessible.

## Options, `ReflectedCaster`

|  Option | Default | Description |
|:---|:---|:---|
| SupportExplicitImplementations | `true` | Supports the case of explicitly implemented interfaces. Priority is given to explicit implementations that have the samme `interface.FullName` as the interface you're Converting to |
| SupportExplicitImplementationsByInterfaceName | `true` | Supports the case of explicitly implemented interfaces. Unlike `SupportExplicitImplementations`, matching is only done on `interface.Name` of the interface you're Converting to |
| SupportSpecificReturnTypes | `false` | Support the cases where an interface defines a more generic return type, than the one the instance returns. Example, interface: `object Method();`, class: `string Method();` |

## Options, `CastToInterface`

|  Option | Default | Description |
|:---|:---|:---|
| AllowMissingFunctions | `false` | Allow cases where an `instance` is missing one or more functions defined in the `interface`. The functions will throw an exception when called |
