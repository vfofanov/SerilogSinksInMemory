using DragoAnt.Assertions;
using Serilog.Events;

namespace DragoAnt.Assertions.Serilog;

public interface StructuredValueAssertions : IAssertionsExtension, ISubjectAssertions<StructureValue>
{
    LogEventPropertyValueAssertions WithProperty(string name, string because = "", params object[] becauseArgs);
}
