using System.Reflection;

namespace DragoAnt.Assertions.Serilog;

public static class SerilogAssertionUtils
{
    private static readonly Lazy<bool> EnsureDefaultAssertionFramework = new(() =>
    {
        _ = DragoAnt.Assertions.AssertionUtils.CreateAssertionsFactory();
        return true;
    });

    public static AssertionsFactory CreateAssertionsFactory()
    {
        _ = EnsureDefaultAssertionFramework.Value;
        var framework = AssertionFramework.Current;
        return CreateAssertionsFactory(framework.Framework, framework.Version.Major);
    }

    public static AssertionsFactory CreateAssertionsFactory(
        AssertionFrameworks assertionFramework,
        int majorVersion,
        string? assemblyLocation = null)
    {
        var factoryType = GetAssertionsFactoryType(assertionFramework, majorVersion, assemblyLocation) ??
                          throw new InvalidOperationException("Unable to load assertion factory");

        var instance = Activator.CreateInstance(factoryType);
        return instance as AssertionsFactory ?? throw new InvalidOperationException("Loaded assertion factory has an unexpected type");
    }

    private static string GetAssertionsAssemblyLocation()
    {
        var assemblyLocation = Path.GetDirectoryName(typeof(SerilogAssertionUtils).Assembly.Location);
        return string.IsNullOrEmpty(assemblyLocation) ? throw new Exception("Unable to determine path to load assemblies from") : assemblyLocation;
    }

    private static Type? GetAssertionsFactoryType(
        AssertionFrameworks assertionFramework,
        int majorVersion,
        string? assemblyLocation = null)
    {
        assemblyLocation ??= GetAssertionsAssemblyLocation();

        var versionedLocation = Path.Combine(
            assemblyLocation,
            $"DragoAnt.Assertions.Serilog.{assertionFramework:G}{majorVersion}.dll");

        if (!File.Exists(versionedLocation))
        {
            throw new InvalidOperationException(
                $"Detected {assertionFramework:G} version {majorVersion} but the Serilog assertions adapter wasn't found on disk");
        }

        var versionedAssembly = Assembly.LoadFrom(versionedLocation);

        return versionedAssembly.GetTypes()
            .SingleOrDefault(t => typeof(AssertionsFactory).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);
    }
}
