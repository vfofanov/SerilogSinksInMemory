// ReSharper disable CoVariantArrayConversion

namespace DragoAnt.Assertions.FrameworkExtension;

public static class AssertionExtensions
{
    public static readonly AssertionFramework AssertionFramework =
        new(AssertionFrameworks.Shouldly, new Version(4, 0),
            (message, condition, because, becauseArgs) =>
                message.Assert(condition, because, becauseArgs));

    public static void Assert(
        this FailMessage failureMessage,
        [DoesNotReturnIf(false)] bool condition,
        string because = "",
        params object?[] becauseArgs)
    {
        if (condition)
        {
            return;
        }

        var message = failureMessage.Message;
        if (failureMessage.Args.Length != 0)
        {
            message = string.Format(message, FormatArgs(failureMessage.Args));
        }

        if (!string.IsNullOrWhiteSpace(because))
        {
            var becauseMessage = becauseArgs.Length == 0
                ? because
                : string.Format(because, becauseArgs);

            message = $"{message} because {becauseMessage}";
        }

        throw new ShouldAssertException(message);
    }

    private static string?[] FormatArgs(IEnumerable args)
        => args.Cast<object>().Select(arg => arg switch
        {
            null => null,
            string str => $"\"{str}\"",
            _ => arg is IEnumerable enumerable ? $"{{{string.Join(", ", FormatArgs(enumerable))}}}" : arg.ToString(),
        }).ToArray();
}