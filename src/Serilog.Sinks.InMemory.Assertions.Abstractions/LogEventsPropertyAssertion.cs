using System.Collections.Generic;
using Serilog.Events;

namespace Serilog.Sinks.InMemory.Assertions;

public interface LogEventsPropertyAssertion :IAssertionsExtension, ISubjectAssertions<IReadOnlyCollection<LogEvent>>
{
    LogEventsAssertions WithValues(params object?[] values);
}