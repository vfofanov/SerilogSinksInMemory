namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class LogEventAssertionImpl : ReferenceTypeAssertions<LogEvent, LogEventAssertionImpl>
{
    public LogEventAssertionImpl(string messageTemplate, LogEvent subject, AssertionChain assertionChain)
        : base(subject, assertionChain)
    {
        _messageTemplate = messageTemplate;
    }

    protected override string Identifier => "log event";

    public void Match(Func<LogEvent, bool> predicate)
    {
        Subject.Should().Match<LogEvent>(o => predicate(o));
    }

    public void Assert(bool condition, FailMessage failureMessage, string because = "", params object[] becauseArgs)
        => CurrentAssertionChain.Assert(condition, failureMessage, because: because, becauseArgs: becauseArgs);
}