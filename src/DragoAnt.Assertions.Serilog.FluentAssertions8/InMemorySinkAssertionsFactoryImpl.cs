namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class AssertionFactoryImpl
{
    public LogEventsSourceAssertions CreateLogEventsSourceAssertions(IReadOnlyCollection<LogEvent> logEvents)
        => new LogEventsSourceAssertionsImpl(logEvents);

    public LogEventsAssertions CreateLogEventsAssertions(string messageTemplate, IReadOnlyCollection<LogEvent> logEvents)
        => new LogEventsAssertionsImpl(messageTemplate, logEvents, AssertionChain.GetOrCreate());

    public PatternLogEventsAssertions CreatePatternLogEventsAssertions(IReadOnlyCollection<LogEvent> logEvents)
        => new PatternLogEventsAssertionsImpl(logEvents, AssertionChain.GetOrCreate());

    public LogEventAssertion CreateLogEventAssertion(string messageTemplate, LogEvent logEvent)
        => new LogEventAssertionImpl(messageTemplate, logEvent, AssertionChain.GetOrCreate());

    public LogEventPropertyValueAssertions CreateLogEventPropertyValueAssertions(
        LogEventAssertion logEventAssertion,
        string propertyName,
        LogEventPropertyValue logEventPropertyValue)
        => new LogEventPropertyValueAssertionsImpl((LogEventAssertionImpl)logEventAssertion, logEventPropertyValue, propertyName);

    public StructuredValueAssertions CreateStructuredValueAssertions(
        LogEventAssertion logEventAssertion,
        StructureValue structureValue,
        string propertyName)
        => new StructuredValueAssertionsImpl((LogEventAssertionImpl)logEventAssertion, structureValue, propertyName);

    public LogEventsPropertyAssertion CreateLogEventsPropertyAssertion(
        LogEventsAssertions logEventsAssertions,
        string propertyName)
        => new LogEventsPropertyAssertionImpl((LogEventsAssertionsImpl)logEventsAssertions, propertyName);
}