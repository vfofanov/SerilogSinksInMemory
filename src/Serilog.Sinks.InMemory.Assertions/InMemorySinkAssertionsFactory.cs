using Serilog.Events;

namespace Serilog.Sinks.InMemory.Assertions;

public interface InMemorySinkAssertionsFactory : AssertionsFactory
{
    InMemorySinkAssertions CreateInMemorySinkAssertions(InMemorySink snapshotInstance);
}
