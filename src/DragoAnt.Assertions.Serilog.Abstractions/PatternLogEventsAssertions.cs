namespace DragoAnt.Assertions.Serilog;

public interface PatternLogEventsAssertions : LogEventsAssertions
{
    LogEventsAssertions Containing(
        string pattern,
        string because = "",
        params object[] becauseArgs);
}
