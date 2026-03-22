using DragoAnt.Assertions;
using FluentAssertions.Execution;

namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

public static class AssertionExtensions
{
    public static readonly AssertionFramework AssertionFramework =
        new(AssertionFrameworks.FluentAssertions, new Version(5, 0));

    public static void Assert(
        this FailMessage failureMessage,
        bool condition,
        string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(condition)
            .FailWith(failureMessage.Message, failureMessage.Args);
    }
}
