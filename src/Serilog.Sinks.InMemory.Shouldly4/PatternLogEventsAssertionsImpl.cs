namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class PatternLogEventsAssertionsImpl
{
    public PatternLogEventsAssertionsImpl(IReadOnlyCollection<LogEvent> logEvents)
        : base(string.Empty, logEvents)
    {
    }
}