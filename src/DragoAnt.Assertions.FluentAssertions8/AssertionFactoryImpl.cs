using DragoAnt.Assertions;

namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

public sealed class PureAssertionFactoryImpl : IAssertionsFactory
{
    public AssertionFramework AssertionFramework => AssertionExtensions.AssertionFramework;
}
