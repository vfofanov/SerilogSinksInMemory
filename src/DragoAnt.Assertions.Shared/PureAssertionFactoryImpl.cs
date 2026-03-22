namespace DragoAnt.Assertions.FrameworkExtension;

public sealed class PureAssertionFactoryImpl : IPureAssertionsFactory
{
    public AssertionFramework AssertionFramework => AssertionExtensions.AssertionFramework;
}
