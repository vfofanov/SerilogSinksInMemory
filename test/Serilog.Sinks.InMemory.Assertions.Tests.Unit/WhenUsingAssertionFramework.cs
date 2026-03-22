using DragoAnt.Assertions;

namespace Serilog.Sinks.InMemory.AssertionsTests;

[Collection(nameof(AssertionFrameworkStateCollection))]
public class WhenUsingAssertionFramework
{
    [Fact]
    public void DefaultFramework_MatchesFactoryFramework()
    {
        EnsureDefaultFrameworkConfigured();
        var frameworkFromFactory = TestInMemorySinkAssertionExtensions.AssertionsFactory.AssertionFramework;
        var defaultFramework = AssertionFramework.Default;

        Assert.Equal(frameworkFromFactory.Framework, defaultFramework.Framework);
        Assert.Equal(frameworkFromFactory.Version, defaultFramework.Version);
    }

    [Fact]
    public void SetCurrent_OverridesCurrentUntilCleared()
    {
        EnsureDefaultFrameworkConfigured();
        var defaultFramework = AssertionFramework.Default;
        var assertCalls = 0;
        var customFramework = new AssertionFramework(
            AssertionFrameworks.Unknown,
            new Version(99, 0),
            (_, _, _, _) => assertCalls++);

        try
        {
            AssertionFramework.SetCurrent(customFramework);

            var current = AssertionFramework.Current;
            Assert.Equal(customFramework.Framework, current.Framework);
            Assert.Equal(customFramework.Version, current.Version);

            current.Assert(condition: true, failureMessage: "custom assertion should run");
            Assert.Equal(1, assertCalls);
        }
        finally
        {
            AssertionFramework.SetCurrent(null);
        }

        var resetCurrent = AssertionFramework.Current;
        Assert.Equal(defaultFramework.Framework, resetCurrent.Framework);
        Assert.Equal(defaultFramework.Version, resetCurrent.Version);
    }

    [Fact]
    public void Assert_DelegatesToConfiguredAssertionWithAllArguments()
    {
        FailMessage? capturedMessage = null;
        bool? capturedCondition = null;
        string? capturedBecause = null;
        object?[]? capturedBecauseArgs = null;

        var framework = new AssertionFramework(
            AssertionFrameworks.Unknown,
            new Version(1, 0),
            (message, condition, because, becauseArgs) =>
            {
                capturedMessage = message;
                capturedCondition = condition;
                capturedBecause = because;
                capturedBecauseArgs = becauseArgs;
            });

        var failMessage = new FailMessage("Expected {0}", "value");
        var becauseArgs = new object[] { "A", 42 };

        framework.Assert(condition: false,
            failureMessage: failMessage,
            because: "because {0} {1}", becauseArgs: becauseArgs);

        Assert.Equal(failMessage.Message, capturedMessage?.Message);
        Assert.Equal(failMessage.Args, capturedMessage?.Args);
        Assert.False(capturedCondition);
        Assert.Equal("because {0} {1}", capturedBecause);
        Assert.Same(becauseArgs, capturedBecauseArgs);
    }

    [Fact]
    public void DefaultFramework_Assert_UsesConfiguredImplementation()
    {
        EnsureDefaultFrameworkConfigured();
        var failure = new FailMessage("Expected default framework failure");

        var thrown = Record.Exception(() => AssertionFramework.Default.Assert(condition: false, failureMessage: failure));

        Assert.NotNull(thrown);
        Assert.Contains("Expected default framework failure", thrown.Message, StringComparison.Ordinal);
    }

    private static void EnsureDefaultFrameworkConfigured()
    {
        _ = TestInMemorySinkAssertionExtensions.AssertionsFactory;
    }
}

[CollectionDefinition(nameof(AssertionFrameworkStateCollection), DisableParallelization = true)]
public sealed class AssertionFrameworkStateCollection
{
}
