namespace DragoAnt.Assertions.FrameworkExtension;

public static class AssertionExtensions
{
    public static readonly AssertionFramework AssertionFramework =
        new(AssertionFrameworks.AwesomeAssertions, new Version(9, 0),
            (message, condition, because, becauseArgs) =>
                AssertionChain.GetOrCreate().Assert(condition, message, because, becauseArgs));

    public static void Assert(
        this AssertionChain assertionChain,
        bool condition,
        FailMessage failureMessage,
        string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(condition)
            .FailWith(failureMessage.Message, failureMessage.Args);
    }
}
