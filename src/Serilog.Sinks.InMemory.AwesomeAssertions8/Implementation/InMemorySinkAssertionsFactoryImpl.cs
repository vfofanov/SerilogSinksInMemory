namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

public sealed partial class AssertionFactoryImpl : AssertionsFactory
{
    public AssertionFramework AssertionFramework => AssertionExtensions.AssertionFramework;
}