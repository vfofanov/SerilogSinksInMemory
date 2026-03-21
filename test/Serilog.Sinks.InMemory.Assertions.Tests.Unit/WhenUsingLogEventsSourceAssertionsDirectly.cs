namespace Serilog.Sinks.InMemory.AssertionsTests;

public class WhenUsingLogEventsSourceAssertionsDirectly
{
    private readonly InMemorySinkAssertionsFactory _factory = InMemorySinkAssertionExtensions.AssertionsFactory;

    [Fact]
    public void GivenLogEvents_HaveMessageFindsMatchingMessage()
    {
        var logEvents = CreateLogEvents(
            ("Hello World", LogEventLevel.Information),
            ("Goodbye World", LogEventLevel.Information));

        var assertions = _factory.CreateLogEventsSourceAssertions(logEvents);

        assertions
            .HaveMessage("Hello World")
            .Appearing()
            .Once();
    }

    [Fact]
    public void GivenLogEvents_HaveMessageWithPredicateFindsMatchingMessage()
    {
        var logEvents = CreateLogEvents(
            ("Error 404", LogEventLevel.Error),
            ("OK 200", LogEventLevel.Information));

        var assertions = _factory.CreateLogEventsSourceAssertions(logEvents);

        assertions
            .HaveMessage(
                e => e.MessageTemplate.Text.Contains("404"),
                "message with 404")
            .Appearing()
            .Once();
    }

    [Fact]
    public void GivenLogEvents_NotHaveMessageSucceedsForAbsentMessage()
    {
        var logEvents = CreateLogEvents(
            ("Hello World", LogEventLevel.Information));

        var assertions = _factory.CreateLogEventsSourceAssertions(logEvents);

        assertions.NotHaveMessage("Nonexistent");
    }

    [Fact]
    public void GivenLogEvents_NotHaveMessageWithPredicateSucceedsForNonMatchingPredicate()
    {
        var logEvents = CreateLogEvents(
            ("Hello World", LogEventLevel.Information));

        var assertions = _factory.CreateLogEventsSourceAssertions(logEvents);

        assertions.NotHaveMessage(
            e => e.MessageTemplate.Text.Contains("404"),
            "message with 404");
    }

    [Fact]
    public void GivenNoLogEvents_HaveMessageFails()
    {
        var logEvents = CreateLogEvents();

        var assertions = _factory.CreateLogEventsSourceAssertions(logEvents);

        Action action = () => assertions.HaveMessage("Hello World");

        action
            .Should()
            .Throw<Exception>()
            .WithMessage("*Hello World*");
    }

    [Fact]
    public void GivenLogEvents_NotHaveMessageWithNullTemplate_FailsWhenEventsExist()
    {
        var logEvents = CreateLogEvents(
            ("Hello", LogEventLevel.Information));

        var assertions = _factory.CreateLogEventsSourceAssertions(logEvents);

        Action action = () => assertions.NotHaveMessage();

        action
            .Should()
            .Throw<Exception>();
    }

    [Fact]
    public void GivenEmptyLogEvents_NotHaveMessageSucceeds()
    {
        var logEvents = CreateLogEvents();

        var assertions = _factory.CreateLogEventsSourceAssertions(logEvents);

        assertions.NotHaveMessage();
    }

    [Fact]
    public void GivenLogEvents_HaveMessageParameterless_ReturnsPatternAssertions()
    {
        var logEvents = CreateLogEvents(
            ("first pattern here", LogEventLevel.Information),
            ("second pattern here", LogEventLevel.Information),
            ("no match", LogEventLevel.Information));

        var assertions = _factory.CreateLogEventsSourceAssertions(logEvents);

        assertions
            .HaveMessage()
            .Containing("pattern")
            .Appearing()
            .Times(2);
    }

    [Fact]
    public void GivenLogEventsWithMultipleSameMessages_TimesCountsCorrectly()
    {
        var logEvents = CreateLogEvents(
            ("Repeated", LogEventLevel.Information),
            ("Repeated", LogEventLevel.Information),
            ("Repeated", LogEventLevel.Information));

        var assertions = _factory.CreateLogEventsSourceAssertions(logEvents);

        assertions
            .HaveMessage("Repeated")
            .Appearing()
            .Times(3);
    }

    [Fact]
    public void GivenLogEventsWithDifferentLevels_WithLevelFiltersCorrectly()
    {
        var logEvents = CreateLogEvents(
            ("Test", LogEventLevel.Warning),
            ("Test", LogEventLevel.Warning));

        var assertions = _factory.CreateLogEventsSourceAssertions(logEvents);

        assertions
            .HaveMessage("Test")
            .Appearing()
            .Times(2)
            .WithLevel(LogEventLevel.Warning);
    }

    [Fact]
    public void GivenLogEventsWithProperties_PropertyAssertionsWork()
    {
        using var sink = new InMemorySink();
        var logger = new LoggerConfiguration()
            .WriteTo.Sink(sink)
            .CreateLogger();

        logger.Information("Hello {Name}", "Alice");
        logger.Information("Hello {Name}", "Bob");

        var assertions = _factory.CreateLogEventsSourceAssertions(sink.LogEvents);

        assertions
            .HaveMessage("Hello {Name}")
            .Appearing()
            .Times(2)
            .WithProperty("Name")
            .WithValues("Alice", "Bob");
    }

    [Fact]
    public void SubjectProperty_ReturnsOriginalCollection()
    {
        var logEvents = CreateLogEvents(
            ("Test", LogEventLevel.Information));

        var assertions = _factory.CreateLogEventsSourceAssertions(logEvents);

        Assert.Same(logEvents, assertions.Subject);
    }

    private static IReadOnlyCollection<LogEvent> CreateLogEvents(
        params (string Template, LogEventLevel Level)[] entries)
    {
        return entries
            .Select(e => new LogEvent(
                DateTimeOffset.UtcNow,
                e.Level,
                null,
                new Serilog.Parsing.MessageTemplateParser().Parse(e.Template),
                Enumerable.Empty<LogEventProperty>()))
            .ToList()
            .AsReadOnly();
    }
}
