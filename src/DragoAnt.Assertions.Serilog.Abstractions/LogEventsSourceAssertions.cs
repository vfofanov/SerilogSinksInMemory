using DragoAnt.Assertions;
using Serilog.Events;

namespace DragoAnt.Assertions.Serilog;

public interface LogEventsSourceAssertions : IAssertionsExtension, ISubjectAssertions<IReadOnlyCollection<LogEvent>>
{
    LogEventsAssertions HaveMessage(
        Func<LogEvent, bool> predicate,
        string? predicateErrorName = null,
        string because = "",
        params object[] becauseArgs);

    LogEventsAssertions HaveMessage(
        string messageTemplate,
        string because = "",
        params object[] becauseArgs);

    PatternLogEventsAssertions HaveMessage();

    void NotHaveMessage(
        string? messageTemplate = null,
        string because = "",
        params object[] becauseArgs);

    void NotHaveMessage(
        Func<LogEvent, bool> predicate,
        string? predicateErrorName = null,
        string because = "",
        params object[] becauseArgs);
}
