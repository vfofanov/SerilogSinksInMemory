namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

partial class LogEventPropertyValueAssertionsImpl : ReferenceTypeAssertions<LogEventPropertyValue, LogEventPropertyValueAssertionsImpl>
{
    public LogEventPropertyValueAssertionsImpl(LogEventAssertionImpl logEventAssertion, LogEventPropertyValue instance, string propertyName)
        : base(instance, logEventAssertion.CurrentAssertionChain)
    {
        _logEventAssertion = logEventAssertion;
        _propertyName = propertyName;
    }

    protected override string Identifier => _propertyName;

    public void Assert(bool condition, FailMessage failureMessage, string because = "", params object[] becauseArgs)
        => CurrentAssertionChain.Assert(condition, failureMessage, because: because, becauseArgs: becauseArgs);
}