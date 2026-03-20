namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class PatternLogEventsAssertionsImpl
{
    public PatternLogEventsAssertionsImpl(IReadOnlyCollection<LogEvent> subjectLogEvents)
        : base(string.Empty, subjectLogEvents)
    {
    }
}