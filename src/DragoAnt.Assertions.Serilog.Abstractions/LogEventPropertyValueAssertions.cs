using Serilog.Events;

namespace DragoAnt.Assertions.Serilog;

public interface LogEventPropertyValueAssertions : IAssertionsExtension, ISubjectAssertions<LogEventPropertyValue>
{
    TValue WhichValue<TValue>();
    StructuredValueAssertions HavingADestructuredObject(string because = "", params object[] becauseArgs);
    LogEventAssertion WithValue(object? value, string because = "", params object[] becauseArgs);
}
