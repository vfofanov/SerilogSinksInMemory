namespace DragoAnt.Assertions;

public interface IAssertionsExtension
{
    void Assert(
        bool condition,
        FailMessage failureMessage,
        string because = "",
        params object[] becauseArgs);

    AssertionFramework AssertionFramework { get; }
}
