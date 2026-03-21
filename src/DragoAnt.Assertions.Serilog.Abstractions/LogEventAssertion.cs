using DragoAnt.Assertions;
using Serilog.Events;

namespace DragoAnt.Assertions.Serilog;

public interface LogEventAssertion : IAssertionsExtension, ISubjectAssertions<LogEvent>
{
    LogEventPropertyValueAssertions WithProperty(string name, string because = "", params object[] becauseArgs);
    LogEventAssertion WithLevel(LogEventLevel level, string because = "", params object[] becauseArgs);
    void Match(Func<LogEvent, bool> predicate);
}
