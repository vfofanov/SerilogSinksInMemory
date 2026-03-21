#nullable enable
namespace Serilog.Sinks.InMemory.Assertions;

public readonly struct AssertionExtension
{
    private readonly IAssertionsExtension _extension;

    public AssertionExtension(IAssertionsExtension extension)
    {
        _extension = extension;
    }

    public AssertionFramework AssertionFramework => _extension.AssertionFramework;

    public void Assert(bool condition, FailMessage failureMessage, string because = "", params object[] becauseArgs) 
        => _extension.Assert(condition, failureMessage, because, becauseArgs);
}