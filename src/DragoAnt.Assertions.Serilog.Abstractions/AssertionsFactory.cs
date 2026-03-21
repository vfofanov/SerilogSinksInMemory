using DragoAnt.Assertions;
using Serilog.Events;

namespace DragoAnt.Assertions.Serilog;

public interface AssertionsFactory : IAssertionsFactory
{
    LogEventsSourceAssertions CreateLogEventsSourceAssertions(IReadOnlyCollection<LogEvent> logEvents);
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
