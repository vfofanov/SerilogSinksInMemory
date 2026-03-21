namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

[ShouldlyMethods]
partial class LogEventsSourceAssertionsImpl : BaseShouldlyAssertions<IReadOnlyCollection<LogEvent>>
{
    public LogEventsSourceAssertionsImpl(IReadOnlyCollection<LogEvent> logEvents)
        : base(logEvents)
    {
    }
}