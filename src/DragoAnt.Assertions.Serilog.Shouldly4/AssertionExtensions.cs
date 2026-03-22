namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

internal static class SerilogAssertionExtensions
{
    public static LogEventsAssertions CreateLogEventsAssertions<TSubject>(
        this BaseShouldlyAssertions<TSubject> parent,
        string pattern,
        IReadOnlyCollection<LogEvent> logEvents)
        => new LogEventsAssertionsImpl(pattern, logEvents);

    public static PatternLogEventsAssertions CreatePatternLogEventsAssertions<TSubject>(
        this BaseShouldlyAssertions<TSubject> parent,
        IReadOnlyCollection<LogEvent> logEvents)
        => new PatternLogEventsAssertionsImpl(logEvents);

    public static StructuredValueAssertions CreateStructuredValueAssertions(
        this LogEventAssertionImpl logEventAssertion,
        StructureValue structureValue,
        string propertyName)
        => new StructuredValueAssertionsImpl(logEventAssertion, structureValue, propertyName);

    public static LogEventPropertyValueAssertions CreateLogEventPropertyValueAssertions(
        this LogEventAssertionImpl parent,
        string name,
        LogEventPropertyValue logEventPropertyValue)
        => new LogEventPropertyValueAssertionsImpl(parent, logEventPropertyValue, name);

    public static LogEventAssertion CreateLogEventAssertion<TSubject>(
        this BaseShouldlyAssertions<TSubject> parent,
        string messageTemplate,
        LogEvent logEvent)
        => new LogEventAssertionImpl(messageTemplate, logEvent);

    public static LogEventsPropertyAssertion CreateLogEventsPropertyAssertion(
        this LogEventsAssertionsImpl parent,
        string propertyName)
        => new LogEventsPropertyAssertionImpl(parent, propertyName);
}