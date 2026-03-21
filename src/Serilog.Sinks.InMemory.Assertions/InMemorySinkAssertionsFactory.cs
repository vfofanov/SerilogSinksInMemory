using Serilog.Events;
using DragoAnt.Assertions.Serilog;

namespace Serilog.Sinks.InMemory.Assertions;

public interface InMemorySinkAssertionsFactory : AssertionsFactory
{
    InMemorySinkAssertions CreateInMemorySinkAssertions(InMemorySink snapshotInstance);
}
