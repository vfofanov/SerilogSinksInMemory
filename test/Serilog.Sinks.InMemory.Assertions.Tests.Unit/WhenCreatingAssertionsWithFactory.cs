namespace Serilog.Sinks.InMemory.AssertionsTests;

public class WhenCreatingAssertionsWithFactory
{
    private readonly InMemorySinkAssertionsFactory _factory = TestInMemorySinkAssertionExtensions.AssertionsFactory;

    [Fact]
    public void GivenLogEvents_CreatePatternLogEventsAssertionsPreservesSubject()
    {
        using var inMemorySink = new InMemorySink();
        var logger = CreateLogger(inMemorySink);

        logger.Information("Hello Alice");
        logger.Information("Hello Bob");

        var logEvents = inMemorySink.LogEvents;

        var assertions = _factory.CreatePatternLogEventsAssertions(logEvents);

        Assert.Same(logEvents, assertions.Subject);

        assertions
            .Containing("Alice")
            .Appearing()
            .Times(1);
    }

    [Fact]
    public void GivenLogEvent_CreateLogEventAssertionPreservesSubject()
    {
        using var inMemorySink = new InMemorySink();
        var logger = CreateLogger(inMemorySink);

        logger.Information("Hello {Name}", "World");

        var logEvent = Assert.Single(inMemorySink.LogEvents);

        var assertions = _factory.CreateLogEventAssertion("Hello {Name}", logEvent);

        Assert.Same(logEvent, assertions.Subject);

        assertions
            .WithProperty("Name")
            .WithValue("World");
    }

    [Fact]
    public void GivenPropertyValue_CreateLogEventPropertyValueAssertionsPreservesSubjectAndParent()
    {
        using var inMemorySink = new InMemorySink();
        var logger = CreateLogger(inMemorySink);

        logger.Information("Hello {Name}", "World");

        var logEvent = Assert.Single(inMemorySink.LogEvents);
        var logEventAssertion = _factory.CreateLogEventAssertion("Hello {Name}", logEvent);
        var propertyValue = logEvent.Properties["Name"];

        var assertions = _factory.CreateLogEventPropertyValueAssertions(logEventAssertion, "Name", propertyValue);

        Assert.Same(propertyValue, assertions.Subject);
        Assert.Same(logEventAssertion, assertions.WithValue("World"));
    }

    [Fact]
    public void GivenStructureValue_CreateStructuredValueAssertionsPreservesSubjectAndSupportsNestedPropertyAssertions()
    {
        using var inMemorySink = new InMemorySink();
        var logger = CreateLogger(inMemorySink);

        logger.Information("Hello {@Placeholder}", new PlaceholderObject());

        var logEvent = Assert.Single(inMemorySink.LogEvents);
        var logEventAssertion = _factory.CreateLogEventAssertion("Hello {@Placeholder}", logEvent);
        var structureValue = Assert.IsType<StructureValue>(logEvent.Properties["Placeholder"]);

        var assertions = _factory.CreateStructuredValueAssertions(logEventAssertion, structureValue, "Placeholder");

        Assert.Same(structureValue, assertions.Subject);

        assertions
            .WithProperty("Name")
            .WithValue("Joe Blogs");
    }

    [Fact]
    public void GivenLogEventsAssertions_CreateLogEventsPropertyAssertionPreservesSubjectAndParent()
    {
        using var inMemorySink = new InMemorySink();
        var logger = CreateLogger(inMemorySink);

        logger.Information("Hello {Name}", "Alice");
        logger.Information("Hello {Name}", "Bob");

        var logEvents = inMemorySink.LogEvents;
        var logEventsAssertions = _factory.CreateLogEventsAssertions("Hello {Name}", logEvents);

        var assertions = _factory.CreateLogEventsPropertyAssertion(logEventsAssertions, "Name");

        Assert.Same(logEvents, assertions.Subject);
        Assert.Same(logEventsAssertions, assertions.WithValues("Alice", "Bob"));
    }

    [Fact]
    public void GivenSnapshotInstance_CreateInMemorySinkAssertionsPreservesSnapshotSubject()
    {
        using var inMemorySink = new InMemorySink();
        var logger = CreateLogger(inMemorySink);
        logger.Information("Hello");

        var snapshot = inMemorySink.Snapshot();
        var assertions = InMemorySinkAssertionExtensions.AssertionsFactory.CreateInMemorySinkAssertions(snapshot);

        assertions
            .HaveMessage("Hello")
            .Appearing()
            .Times(1);
    }

    [Fact]
    public void GivenMutableSink_CreateInMemorySinkAssertionsFromSnapshotKeepsStableView()
    {
        using var inMemorySink = new InMemorySink();
        var logger = CreateLogger(inMemorySink);

        logger.Information("Hello");
        var assertions = InMemorySinkAssertionExtensions.AssertionsFactory.CreateInMemorySinkAssertionsFromSnapshot(inMemorySink);

        logger.Information("Hello");

        assertions
            .HaveMessage("Hello")
            .Appearing()
            .Times(1);
    }

    private static ILogger CreateLogger(InMemorySink inMemorySink) =>
        new LoggerConfiguration()
            .WriteTo.Sink(inMemorySink)
            .CreateLogger();

    private sealed class PlaceholderObject
    {
        public string Name { get; set; } = "Joe Blogs";
    }
}
