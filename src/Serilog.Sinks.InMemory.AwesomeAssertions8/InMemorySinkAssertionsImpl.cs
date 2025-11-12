#nullable enable
namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class InMemorySinkAssertionsImpl : ReferenceTypeAssertions<InMemorySink, InMemorySinkAssertionsImpl>
{
    public InMemorySinkAssertionsImpl(InMemorySink snapshotInstance)
        : base(snapshotInstance, AssertionChain.GetOrCreate())
    {
    }

    protected override string Identifier => nameof(InMemorySink);

    public void Assert(bool condition, FailMessage failureMessage, string because = "", params object[] becauseArgs)
        => CurrentAssertionChain.Assert(condition, failureMessage, because: because, becauseArgs: becauseArgs);
}