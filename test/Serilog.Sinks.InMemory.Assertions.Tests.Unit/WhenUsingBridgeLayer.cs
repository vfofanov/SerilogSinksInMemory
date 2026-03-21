using DragoAnt.Assertions;

namespace Serilog.Sinks.InMemory.AssertionsTests;

public class WhenUsingBridgeLayer
{
    private readonly InMemorySinkAssertionsFactory _factory = InMemorySinkAssertionExtensions.AssertionsFactory;

    [Fact]
    public void BridgeFactory_AssertionFramework_IsNotUnknown()
    {
        var framework = _factory.AssertionFramework;

        Assert.NotEqual(AssertionFrameworks.Unknown, framework.Framework);
        Assert.NotNull(framework.Version);
    }

    [Fact]
    public void BridgeFactory_CreateLogEventsSourceAssertions_ReturnsWorkingAssertions()
    {
        using var sink = new InMemorySink();
        var logger = CreateLogger(sink);

        logger.Information("Hello World");
        logger.Information("Goodbye World");

        var assertions = _factory.CreateLogEventsSourceAssertions(sink.LogEvents);

        Assert.Same(sink.LogEvents, assertions.Subject);

        assertions
            .HaveMessage("Hello World")
            .Appearing()
            .Once();
    }

    [Fact]
    public void BridgeFactory_CreateInMemorySinkAssertions_SinkSubjectIsSinkSnapshot()
    {
        using var sink = new InMemorySink();
        var logger = CreateLogger(sink);

        logger.Information("Test");

        var snapshot = sink.Snapshot();
        var assertions = _factory.CreateInMemorySinkAssertions(snapshot);
        var sinkSubject = ((ISubjectAssertions<InMemorySink>)assertions).Subject;

        Assert.Same(snapshot, sinkSubject);
    }

    [Fact]
    public void BridgeAssertions_InMemorySinkSubject_IsSameAsSinkPassedIn()
    {
        using var sink = new InMemorySink();
        var logger = CreateLogger(sink);

        logger.Information("Test");

        var snapshot = sink.Snapshot();
        var assertions = _factory.CreateInMemorySinkAssertions(snapshot);
        var sinkSubject = ((ISubjectAssertions<InMemorySink>)assertions).Subject;

        Assert.Same(snapshot, sinkSubject);
    }

    [Fact]
    public void BridgeAssertions_LogEventsSubject_ContainsSnapshotLogEvents()
    {
        using var sink = new InMemorySink();
        var logger = CreateLogger(sink);

        logger.Information("Test");

        var snapshot = sink.Snapshot();
        var assertions = _factory.CreateInMemorySinkAssertions(snapshot);
        var logEventsSubject = ((ISubjectAssertions<IReadOnlyCollection<LogEvent>>)assertions).Subject;

        Assert.Equal(snapshot.LogEvents.Count, logEventsSubject.Count);
    }

    [Fact]
    public void BridgeAssertions_AssertionFramework_MatchesFactory()
    {
        using var sink = new InMemorySink();
        var logger = CreateLogger(sink);

        logger.Information("Test");

        var snapshot = sink.Snapshot();
        var assertions = _factory.CreateInMemorySinkAssertions(snapshot);

        Assert.Equal(_factory.AssertionFramework.Framework, assertions.AssertionFramework.Framework);
        Assert.Equal(_factory.AssertionFramework.Version, assertions.AssertionFramework.Version);
    }

    [Fact]
    public void BridgeAssertions_HaveMessage_DelegatesToUnderlyingAssertions()
    {
        using var sink = new InMemorySink();
        var logger = CreateLogger(sink);

        logger.Information("Message A");
        logger.Information("Message B");

        var snapshot = sink.Snapshot();
        var assertions = _factory.CreateInMemorySinkAssertions(snapshot);

        assertions
            .HaveMessage("Message A")
            .Appearing()
            .Once();

        assertions
            .HaveMessage("Message B")
            .Appearing()
            .Once();
    }

    [Fact]
    public void BridgeAssertions_NotHaveMessage_DelegatesToUnderlyingAssertions()
    {
        using var sink = new InMemorySink();
        var logger = CreateLogger(sink);

        logger.Information("Existing");

        var snapshot = sink.Snapshot();
        var assertions = _factory.CreateInMemorySinkAssertions(snapshot);

        assertions.NotHaveMessage("NonExistent");
    }

    [Fact]
    public void BridgeAssertions_HaveMessageWithPredicate_DelegatesToUnderlyingAssertions()
    {
        using var sink = new InMemorySink();
        var logger = CreateLogger(sink);

        logger.Information("Error 404");

        var snapshot = sink.Snapshot();
        var assertions = _factory.CreateInMemorySinkAssertions(snapshot);

        assertions
            .HaveMessage(
                logEvent => logEvent.MessageTemplate.Text.Contains("404"),
                "message with 404")
            .Appearing()
            .Once();
    }

    [Fact]
    public void BridgeAssertions_NotHaveMessageWithPredicate_DelegatesToUnderlyingAssertions()
    {
        using var sink = new InMemorySink();
        var logger = CreateLogger(sink);

        logger.Information("OK 200");

        var snapshot = sink.Snapshot();
        var assertions = _factory.CreateInMemorySinkAssertions(snapshot);

        assertions.NotHaveMessage(
            logEvent => logEvent.MessageTemplate.Text.Contains("404"),
            "message with 404");
    }

    [Fact]
    public void BridgeAssertions_HaveMessageParameterless_ReturnsPatternAssertions()
    {
        using var sink = new InMemorySink();
        var logger = CreateLogger(sink);

        logger.Information("padding pattern padding");

        var snapshot = sink.Snapshot();
        var assertions = _factory.CreateInMemorySinkAssertions(snapshot);

        assertions
            .HaveMessage()
            .Containing("pattern")
            .Appearing()
            .Times(1);
    }

    [Fact]
    public void ShouldExtensionMethod_CreatesWorkingBridgeAssertions()
    {
        using var sink = new InMemorySink();
        var logger = CreateLogger(sink);

        logger.Information("Hello {Name}", "World");

        var assertions = sink.Should();

        assertions
            .HaveMessage("Hello {Name}")
            .Appearing()
            .Once()
            .WithProperty("Name")
            .WithValue("World");
    }

    [Fact]
    public void BridgeFactory_AllFactoryMethods_DelegateCorrectly()
    {
        using var sink = new InMemorySink();
        var logger = CreateLogger(sink);

        logger.Information("Hello {Name}", "World");

        var logEvents = sink.LogEvents;
        var logEvent = logEvents.First();

        var logEventsAssertions = _factory.CreateLogEventsAssertions("Hello {Name}", logEvents);
        Assert.Same(logEvents, logEventsAssertions.Subject);

        var patternAssertions = _factory.CreatePatternLogEventsAssertions(logEvents);
        Assert.Same(logEvents, patternAssertions.Subject);

        var logEventAssertion = _factory.CreateLogEventAssertion("Hello {Name}", logEvent);
        Assert.Same(logEvent, logEventAssertion.Subject);

        var propertyValue = logEvent.Properties["Name"];
        var propAssertions = _factory.CreateLogEventPropertyValueAssertions(logEventAssertion, "Name", propertyValue);
        Assert.Same(propertyValue, propAssertions.Subject);

        var propertyAssertion = _factory.CreateLogEventsPropertyAssertion(logEventsAssertions, "Name");
        Assert.Same(logEvents, propertyAssertion.Subject);
    }

    private static ILogger CreateLogger(InMemorySink sink) =>
        new LoggerConfiguration()
            .WriteTo.Sink(sink)
            .CreateLogger();
}
