namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class LogEventAssertionImpl : ReferenceTypeAssertions<LogEvent, LogEventAssertionImpl>
{
    public LogEventAssertionImpl(string messageTemplate, LogEvent subject)
        : base(subject)
    {
        _messageTemplate = messageTemplate;
    }

    protected override string Identifier => "log event";

    public void Match(Func<LogEvent, bool> predicate)
    {
        Subject.Should().Match<LogEvent>(o => predicate(o));
    }

    public void Assert(bool condition, FailMessage failureMessage, string because = "", params object[] becauseArgs)
        => failureMessage.Assert(condition, because: because, becauseArgs: becauseArgs);
}