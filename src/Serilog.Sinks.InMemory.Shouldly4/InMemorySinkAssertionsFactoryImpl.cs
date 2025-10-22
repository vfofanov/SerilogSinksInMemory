using Serilog.Sinks.InMemory.Assertions;

namespace Serilog.Sinks.InMemory.Shouldly4
{
    public sealed class InMemorySinkAssertionsFactoryImpl : InMemorySinkAssertionsFactory
    {
        public InMemorySinkAssertions CreateInMemorySinkAssertions(InMemorySink snapshotInstance)
        {
            return new InMemorySinkAssertionsImpl(snapshotInstance);
        }
    }
}