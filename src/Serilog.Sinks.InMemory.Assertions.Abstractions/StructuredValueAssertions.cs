using DragoAnt.Assertions;
using Serilog.Events;

namespace Serilog.Sinks.InMemory.Assertions;

public interface StructuredValueAssertions : IAssertionsExtension, ISubjectAssertions<StructureValue>
{
    LogEventPropertyValueAssertions WithProperty(string name, string because = "", params object[] becauseArgs);
}