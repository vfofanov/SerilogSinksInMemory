using System.Reflection;
using System.Runtime.CompilerServices;

namespace DragoAnt.Assertions.Serilog;

public static class SerilogAssertionUtils
{
    private static readonly Lazy<bool> EnsureDefaultAssertionFramework = new(() =>
    {
        RuntimeHelpers.RunClassConstructor(typeof(DragoAnt.Assertions.AssertionUtils).TypeHandle);
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

        foreach (var baseDir in GetAssertionProbeDirectories(assemblyLocation))
        {
            var versionedLocation = Path.Combine(
                baseDir,
                $"DragoAnt.Assertions.Serilog.{assertionFramework:G}{majorVersion}.dll");

            if (!File.Exists(versionedLocation))
            {
                continue;
            }

            var versionedAssembly = Assembly.LoadFrom(versionedLocation);
            var factoryType = versionedAssembly.GetTypes()
                .SingleOrDefault(t => typeof(AssertionsFactory).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

            if (factoryType != null)
            {
                return factoryType;
            }
        }

        throw new InvalidOperationException(
            $"Detected {assertionFramework:G} version {majorVersion} but the Serilog assertions adapter wasn't found on disk");
    }

    /// <summary>
    /// NuGet consumers resolve adapters next to the test assembly (BaseDirectory), not only
    /// next to <see cref="DragoAnt.Assertions.Serilog"/> in the package lib folder.
    /// </summary>
    private static IEnumerable<string> GetAssertionProbeDirectories(string packageLibDirectory)
    {
        yield return packageLibDirectory;

        var baseDir = AppContext.BaseDirectory;
        if (!string.IsNullOrEmpty(baseDir))
        {
            yield return baseDir;
        }
    }
}
