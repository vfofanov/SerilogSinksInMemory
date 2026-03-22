namespace DragoAnt.Assertions.FrameworkExtension;

public static class AssertionExtensions
{
    public static readonly AssertionFramework AssertionFramework =
        new(AssertionFrameworks.FluentAssertions, new Version(7, 0),
            (message, condition, because, becauseArgs) =>
                message.Assert(condition, because, becauseArgs));

    public static void Assert(
        this FailMessage failureMessage,
        [DoesNotReturnIf(false)]bool condition,
        string because = "",
        params object?[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(condition)
            .FailWith(failureMessage.Message, failureMessage.Args);
    }
}
