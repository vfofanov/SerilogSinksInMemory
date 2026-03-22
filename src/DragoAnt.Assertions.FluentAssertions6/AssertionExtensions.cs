using DragoAnt.Assertions;
using FluentAssertions.Execution;

namespace DragoAnt.Assertions.FrameworkExtension;

public static class AssertionExtensions
{
    public static readonly AssertionFramework AssertionFramework =
        new(AssertionFrameworks.FluentAssertions, new Version(6, 0));

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
