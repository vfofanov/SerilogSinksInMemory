using DragoAnt.Assertions;
using DragoAnt.Assertions.Serilog;
using Serilog.Events;

namespace Serilog.Sinks.InMemory.Assertions;

internal sealed class InMemorySinkAssertionsBridge : InMemorySinkAssertions
{
    private readonly LogEventsSourceAssertions _assertions;

    public InMemorySinkAssertionsBridge(InMemorySink subject, LogEventsSourceAssertions assertions)
    {
        Subject = subject;
        _assertions = assertions;
    }

    public InMemorySink Subject { get; }
    IReadOnlyCollection<LogEvent> ISubjectAssertions<IReadOnlyCollection<LogEvent>>.Subject => _assertions.Subject;

    public AssertionFramework AssertionFramework => _assertions.AssertionFramework;

    public void Assert(bool condition, FailMessage failureMessage, string because = "", params object[] becauseArgs)
        => _assertions.Assert(condition, failureMessage, because, becauseArgs);

    public LogEventsAssertions HaveMessage(
        Func<LogEvent, bool> predicate,
        string? predicateErrorName = null,
        string because = "",
        params object[] becauseArgs)
        => _assertions.HaveMessage(predicate, predicateErrorName, because, becauseArgs);

    public LogEventsAssertions HaveMessage(string messageTemplate, string because = "", params object[] becauseArgs)
        => _assertions.HaveMessage(messageTemplate, because, becauseArgs);

    public PatternLogEventsAssertions HaveMessage()
        => _assertions.HaveMessage();

    public void NotHaveMessage(string? messageTemplate = null, string because = "", params object[] becauseArgs)
        => _assertions.NotHaveMessage(messageTemplate, because, becauseArgs);

    public void NotHaveMessage(
        Func<LogEvent, bool> predicate,
        string? predicateErrorName = null,
        string because = "",
        params object[] becauseArgs)
        => _assertions.NotHaveMessage(predicate, predicateErrorName, because, becauseArgs);
}
