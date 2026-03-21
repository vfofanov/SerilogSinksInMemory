using DragoAnt.Assertions;

namespace Serilog.Sinks.InMemory.Assertions;

public interface InMemorySinkAssertions : LogEventsSourceAssertions, ISubjectAssertions<InMemorySink>
{
}
