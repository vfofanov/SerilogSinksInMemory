#nullable enable
namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class InMemorySinkAssertionsImpl : ReferenceTypeAssertions<InMemorySink, InMemorySinkAssertionsImpl>
{
    public InMemorySinkAssertionsImpl(InMemorySink snapshotInstance)
        : base(snapshotInstance)
    {
    }

    protected override string Identifier => nameof(InMemorySink);

    public void Assert(bool condition, FailMessage failureMessage, string because = "", params object[] becauseArgs)
        => failureMessage.Assert(condition, because: because, becauseArgs: becauseArgs);
}