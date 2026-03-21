namespace DragoAnt.Assertions;

/// <summary>
/// Assertion extensibility extension.
/// </summary>
public static class AssertionsExtensions
{
    public static AssertionExtension ToAssertion(
        this IAssertionsExtension assertions) =>
        assertions == null
            ? throw new ArgumentNullException(nameof(assertions))
            : new AssertionExtension(assertions);
}
