using Serilog.Events;

namespace Serilog.Sinks.InMemory.Assertions;

public interface InMemorySinkAssertionsFactory
{
    AssertionFramework AssertionFramework { get; }
    InMemorySinkAssertions CreateInMemorySinkAssertions(InMemorySink snapshotInstance);
    LogEventsAssertions CreateLogEventsAssertions(string messageTemplate, IReadOnlyCollection<LogEvent> logEvents);
    PatternLogEventsAssertions CreatePatternLogEventsAssertions(IReadOnlyCollection<LogEvent> logEvents);
    LogEventAssertion CreateLogEventAssertion(string messageTemplate, LogEvent logEvent);

    LogEventPropertyValueAssertions CreateLogEventPropertyValueAssertions(
        LogEventAssertion logEventAssertion,
        string propertyName,
        LogEventPropertyValue logEventPropertyValue);

    StructuredValueAssertions CreateStructuredValueAssertions(
        LogEventAssertion logEventAssertion,
        StructureValue structureValue,
        string propertyName);

    LogEventsPropertyAssertion CreateLogEventsPropertyAssertion(
        LogEventsAssertions logEventsAssertions,
        string propertyName);
}