using DragoAnt.Assertions;
using DragoAnt.Assertions.Serilog;
using Serilog.Events;

namespace Serilog.Sinks.InMemory.Assertions;

internal sealed class InMemorySinkAssertionsFactoryBridge : InMemorySinkAssertionsFactory
{
    private readonly AssertionsFactory _factory;

    public InMemorySinkAssertionsFactoryBridge(AssertionsFactory factory)
    {
        _factory = factory;
    }

    public AssertionFramework AssertionFramework => _factory.AssertionFramework;

    public InMemorySinkAssertions CreateInMemorySinkAssertions(InMemorySink snapshotInstance)
        => new InMemorySinkAssertionsBridge(snapshotInstance, _factory.CreateLogEventsSourceAssertions(snapshotInstance.LogEvents));

    public LogEventsSourceAssertions CreateLogEventsSourceAssertions(IReadOnlyCollection<LogEvent> logEvents)
        => _factory.CreateLogEventsSourceAssertions(logEvents);

    public LogEventsAssertions CreateLogEventsAssertions(string messageTemplate, IReadOnlyCollection<LogEvent> logEvents)
        => _factory.CreateLogEventsAssertions(messageTemplate, logEvents);

    public PatternLogEventsAssertions CreatePatternLogEventsAssertions(IReadOnlyCollection<LogEvent> logEvents)
        => _factory.CreatePatternLogEventsAssertions(logEvents);

    public LogEventAssertion CreateLogEventAssertion(string messageTemplate, LogEvent logEvent)
        => _factory.CreateLogEventAssertion(messageTemplate, logEvent);

    public LogEventPropertyValueAssertions CreateLogEventPropertyValueAssertions(
        LogEventAssertion logEventAssertion,
        string propertyName,
        LogEventPropertyValue logEventPropertyValue)
        => _factory.CreateLogEventPropertyValueAssertions(logEventAssertion, propertyName, logEventPropertyValue);

    public StructuredValueAssertions CreateStructuredValueAssertions(
        LogEventAssertion logEventAssertion,
        StructureValue structureValue,
        string propertyName)
        => _factory.CreateStructuredValueAssertions(logEventAssertion, structureValue, propertyName);

    public LogEventsPropertyAssertion CreateLogEventsPropertyAssertion(
        LogEventsAssertions logEventsAssertions,
        string propertyName)
        => _factory.CreateLogEventsPropertyAssertion(logEventsAssertions, propertyName);
}
