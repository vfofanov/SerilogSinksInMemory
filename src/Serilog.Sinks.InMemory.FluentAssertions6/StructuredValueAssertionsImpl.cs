namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class StructuredValueAssertionsImpl : ReferenceTypeAssertions<StructureValue, StructuredValueAssertionsImpl>
{
    public StructuredValueAssertionsImpl(LogEventAssertionImpl logEventAssertion, StructureValue subject, string propertyName)
        : base(subject)
    {
        _propertyName = propertyName;
        _logEventAssertion = logEventAssertion;
    }

    protected override string Identifier => _propertyName;

    public void Assert(bool condition, FailMessage failureMessage, string because = "", params object[] becauseArgs)
        => failureMessage.Assert(condition, because: because, becauseArgs: becauseArgs);
}