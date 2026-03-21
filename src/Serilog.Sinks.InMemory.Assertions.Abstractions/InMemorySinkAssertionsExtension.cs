namespace Serilog.Sinks.InMemory.Assertions;

/// <summary>
/// InMemory logger assertion extensibility extension.
/// </summary>
public static class InMemorySinkAssertionsExtensions
{
    public static AssertionExtension ToAssertion(
        this IAssertionsExtension assertions) =>
        assertions == null
            ? throw new ArgumentNullException(nameof(assertions))
            : new AssertionExtension(assertions);
}