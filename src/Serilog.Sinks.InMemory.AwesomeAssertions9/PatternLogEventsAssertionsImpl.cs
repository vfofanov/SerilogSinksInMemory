namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class PatternLogEventsAssertionsImpl
{
    public PatternLogEventsAssertionsImpl(IReadOnlyCollection<LogEvent> subjectLogEvents, AssertionChain assertionChain)
        : base(string.Empty, subjectLogEvents, assertionChain)
    {
    }
}