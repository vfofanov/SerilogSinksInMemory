namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class LogEventsSourceAssertionsImpl : BaseAssertions<IReadOnlyCollection<LogEvent>, LogEventsSourceAssertionsImpl>
{
    public LogEventsSourceAssertionsImpl(IReadOnlyCollection<LogEvent> logEvents)
        : base(logEvents)
    {
    }

    protected override string Identifier => "log events source";
}