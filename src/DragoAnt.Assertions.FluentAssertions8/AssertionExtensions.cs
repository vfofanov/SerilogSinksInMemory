namespace DragoAnt.Assertions.FrameworkExtension;

public static class AssertionExtensions
{
    public static readonly AssertionFramework AssertionFramework =
        new(AssertionFrameworks.FluentAssertions, new Version(8, 0),
            (message, condition, because, becauseArgs) =>
                AssertionChain.GetOrCreate().Assert(condition, message, because, becauseArgs));

    public static void Assert(
        this AssertionChain assertionChain,
        [DoesNotReturnIf(false)]bool condition,
        FailMessage failureMessage,
        string because = "",
        params object?[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(condition)
            .FailWith(failureMessage.Message, failureMessage.Args);
    }
}
