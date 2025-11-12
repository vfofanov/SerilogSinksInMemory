namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class LogEventsAssertionsImpl : ReferenceTypeAssertions<IReadOnlyCollection<LogEvent>, LogEventsAssertionsImpl>
{
    public LogEventsAssertionsImpl(string messageTemplate, IReadOnlyCollection<LogEvent> matches, AssertionChain assertionChain)
        : base(matches, assertionChain)
    {
        _messageTemplate = messageTemplate;
    }

    protected override string Identifier => "log events";

    public void Assert(bool condition, FailMessage failureMessage, string because = "", params object[] becauseArgs)
        => CurrentAssertionChain.Assert(condition, failureMessage, because: because, becauseArgs: becauseArgs);
}