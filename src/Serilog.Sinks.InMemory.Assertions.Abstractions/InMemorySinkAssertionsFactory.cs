namespace Serilog.Sinks.InMemory.Assertions
{
    public interface InMemorySinkAssertionsFactory
    {
        InMemorySinkAssertions CreateInMemorySinkAssertions(InMemorySink snapshotInstance);
    }
}