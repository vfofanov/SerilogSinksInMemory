using DragoAnt.Assertions;
using DragoAnt.Assertions.Serilog;

namespace Serilog.Sinks.InMemory.Assertions;

public interface InMemorySinkAssertions : LogEventsSourceAssertions, ISubjectAssertions<InMemorySink>
{
}
