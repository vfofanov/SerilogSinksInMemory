using Serilog.Sinks.InMemory.Assertions;

namespace Serilog.Sinks.InMemory.AwesomeAssertions8
{
    public sealed class InMemorySinkAssertionsFactoryImpl : InMemorySinkAssertionsFactory
    {
        public InMemorySinkAssertions CreateInMemorySinkAssertions(InMemorySink snapshotInstance)
        {
            return new InMemorySinkAssertionsImpl(snapshotInstance);
        }
    }
}