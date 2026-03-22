using AwesomeAssertions.Execution;
using DragoAnt.Assertions;

namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

public static class AssertionExtensions
{
    public static readonly AssertionFramework AssertionFramework =
        new(AssertionFrameworks.AwesomeAssertions, new Version(9, 0));

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
