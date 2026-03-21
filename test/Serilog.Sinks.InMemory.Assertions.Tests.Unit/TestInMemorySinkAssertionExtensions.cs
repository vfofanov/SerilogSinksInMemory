namespace Serilog.Sinks.InMemory.AssertionsTests;

public static class TestInMemorySinkAssertionExtensions
{
    public static InMemorySinkAssertionsFactory AssertionsFactory { get; }

    static TestInMemorySinkAssertionExtensions()
    {
        AssertionsFactory = InMemorySinkAssertionExtensions.AssertionsFactory;
    }

    public static InMemorySinkAssertions Should(this InMemorySink instance)
        => InMemorySinkAssertionExtensions.Should(instance);
}