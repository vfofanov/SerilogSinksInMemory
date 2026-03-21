using DragoAnt.Assertions;
using Serilog.Events;

namespace Serilog.Sinks.InMemory.Assertions;

public interface LogEventsAssertions : IAssertionsExtension, ISubjectAssertions<IReadOnlyCollection<LogEvent>>
{
    LogEventsAssertions Appearing();
    LogEventsAssertions Times(int number, string because = "", params object[] becauseArgs);

    LogEventsAssertions WithLevel(LogEventLevel level, string because = "", params object[] becauseArgs);
    LogEventAssertion Once(string because = "", params object[] becauseArgs);
    LogEventsPropertyAssertion WithProperty(string propertyName, string because = "", params object[] becauseArgs);
}