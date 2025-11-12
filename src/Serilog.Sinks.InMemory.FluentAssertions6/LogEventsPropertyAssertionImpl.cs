namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class LogEventsPropertyAssertionImpl : ReferenceTypeAssertions<IReadOnlyCollection<LogEvent>, LogEventsPropertyAssertionImpl>
{
    public LogEventsPropertyAssertionImpl(LogEventsAssertionsImpl logEventsAssertions, string propertyName)
        : base(logEventsAssertions.Subject)
    {
        _logEventsAssertions = logEventsAssertions;
        _propertyName = propertyName;
    }

    protected override string Identifier => _propertyName;

    public void Assert(bool condition, FailMessage failureMessage, string because = "", params object[] becauseArgs)
        => failureMessage.Assert(condition, because: because, becauseArgs: becauseArgs);
}