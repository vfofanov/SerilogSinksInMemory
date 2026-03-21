using DragoAnt.Assertions;
using Serilog.Events;

namespace DragoAnt.Assertions.Serilog;

public interface LogEventsPropertyAssertion : IAssertionsExtension, ISubjectAssertions<IReadOnlyCollection<LogEvent>>
{
    LogEventsAssertions WithValues(params object?[] values);
}
