namespace DragoAnt.Assertions;

public readonly struct AssertionFramework
{
    private static readonly AsyncLocal<AssertionFramework?> LocalCurrent = new();
    private static readonly object DefaultLock = new();
    private static AssertionFramework? _default;
    private static Func<AssertionFramework>? _defaultFactory;
    
    private readonly Action<FailMessage, bool, string, object?[]> _assert;

    public AssertionFramework(AssertionFrameworks framework, Version version, Action<FailMessage, bool, string, object?[]> assert)
    {
        Framework = framework;
        Version = version;
        _assert = assert;
    }

    public static AssertionFramework Default
    {
        get
        {
            if (_default.HasValue)
            {
                return _default.Value;
            }

            lock (DefaultLock)
            {
                if (_default.HasValue)
                {
                    return _default.Value;
                }

                var defaultFactory = _defaultFactory
                    ?? throw new InvalidOperationException(
                        "Default assertion framework is not configured. Make sure DragoAnt.Assertions is referenced.");

                _default = defaultFactory();
                return _default.Value;
            }
        }
    }

    public static AssertionFramework Current => LocalCurrent.Value ?? Default;

    public static void SetCurrent(AssertionFramework? framework)
    {
        LocalCurrent.Value = framework;
    }

    public static void SetDefault(Func<AssertionFramework> defaultFactory)
    {
        if (defaultFactory == null)
        {
            throw new ArgumentNullException(nameof(defaultFactory));
        }

        lock (DefaultLock)
        {
            _defaultFactory = defaultFactory;
            _default = null;
        }
    }

    public static void SetDefault(AssertionFramework framework)
    {
        lock (DefaultLock)
        {
            _default = framework;
            _defaultFactory = null;
        }
    }

    public AssertionFrameworks Framework { get; }

    public Version Version { get; }
    
    public void Assert(
        [DoesNotReturnIf(false)] bool condition,
        FailMessage failureMessage,
        string because = "",
        params object?[] becauseArgs) => _assert(failureMessage, condition, because, becauseArgs);
}
