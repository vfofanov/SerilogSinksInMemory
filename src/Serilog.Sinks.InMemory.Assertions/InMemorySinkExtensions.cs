using DragoAnt.Assertions.Serilog;

namespace Serilog.Sinks.InMemory.Assertions;

public static class InMemorySinkAssertionExtensions
{
    public static InMemorySinkAssertionsFactory AssertionsFactory { get; }

    static InMemorySinkAssertionExtensions()
    {
        var assertionsFactory = SerilogAssertionUtils.CreateAssertionsFactory();

        AssertionsFactory = new InMemorySinkAssertionsFactoryBridge(assertionsFactory);
    }

    public static InMemorySinkAssertions Should(this InMemorySink instance)
        => AssertionsFactory.CreateInMemorySinkAssertionsFromSnapshot(instance);
}