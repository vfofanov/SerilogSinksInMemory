namespace Serilog.Sinks.InMemory.Assertions;

public static class InMemorySinkAssertionsFactoryExtensions
{
    public static InMemorySinkAssertions CreateInMemorySinkAssertionsFromSnapshot(this InMemorySinkAssertionsFactory factory, InMemorySink instance)
        => factory.CreateInMemorySinkAssertions(instance.Snapshot());
}
