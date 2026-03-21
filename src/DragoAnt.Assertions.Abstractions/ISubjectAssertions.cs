namespace DragoAnt.Assertions;

public interface ISubjectAssertions<out T>
{
    T Subject { get; }
}
