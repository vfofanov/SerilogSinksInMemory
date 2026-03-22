using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DragoAnt.Assertions;

public static class AssertionUtils
{
    static AssertionUtils()
    {
        AssertionFramework.SetDefault(GetDefaultAssertionFramework);
    }

    public static IAssertionsFactory CreateAssertionsFactory()
    {
        var (assertionFramework, majorVersion, assemblyLocation) = GetAssertionsFramework();
        return CreateAssertionsFactory(assertionFramework, majorVersion, assemblyLocation);
    }

    public static IAssertionsFactory CreateAssertionsFactory(
        AssertionFrameworks assertionFramework,
        int majorVersion,
        string? assemblyLocation = null)
    {
        var factoryType = GetAssertionsFactoryType(assertionFramework, majorVersion, assemblyLocation) ??
                          throw new InvalidOperationException("Unable to load assertion factory");

        var instance = Activator.CreateInstance(factoryType);
        return instance as IAssertionsFactory ?? throw new InvalidOperationException("Loaded assertion factory has an unexpected type");
    }

    internal static (AssertionFrameworks Framework, int MajorVersion, string AssemblyLocation) GetAssertionsFramework()
    {
        var assemblyLocation = GetAssertionsAssemblyLocation();

        AssertionFrameworks? assertionFramework = null;
        int? majorVersion = null;

        // Order is important here, first check the loaded assemblies before
        // looking on disk because otherwise we might load FluentAssertions from disk
        // while Shouldly is already loaded into the AppDomain and that's the one we
        // should be using.
        // AwesomeAssertions ships as FluentAssertions.dll (v8) or AwesomeAssertions.dll (v9+);
        // detect Awesome before FluentAssertions so the fork is not mistaken for upstream FA.
        if (IsAwesomeAssertionsAlreadyLoadedIntoDomain(out var awesomeAssertionsAssembly))
        {
            assertionFramework = AssertionFrameworks.AwesomeAssertions;
            majorVersion = awesomeAssertionsAssembly.GetName().Version!.Major;
        }
        else if (IsFluentAssertionsAlreadyLoadedIntoDomain(out var fluentAssertionsAssembly))
        {
            assertionFramework = AssertionFrameworks.FluentAssertions;
            majorVersion = fluentAssertionsAssembly.GetName().Version!.Major;
        }
        else if (IsShouldlyAlreadyLoadedIntoDomain(out var shouldlyAssembly))
        {
            assertionFramework = AssertionFrameworks.Shouldly;
            majorVersion = shouldlyAssembly.GetName().Version!.Major;
        }
        else if (IsAwesomeAssertionsAvailableOnDisk(assemblyLocation, out var awesomeAssertionsOnDiskAssembly))
        {
            assertionFramework = AssertionFrameworks.AwesomeAssertions;
            majorVersion = awesomeAssertionsOnDiskAssembly.GetName().Version!.Major;
        }
        else if (IsFluentAssertionsAvailableOnDisk(assemblyLocation, out var fluentAssertionsOnDiskAssembly))
        {
            assertionFramework = AssertionFrameworks.FluentAssertions;
            majorVersion = fluentAssertionsOnDiskAssembly.GetName().Version!.Major;
        }
        else if (IsShouldlyAvailableOnDisk(assemblyLocation, out var shouldlyOnDiskAssembly))
        {
            assertionFramework = AssertionFrameworks.Shouldly;
            majorVersion = shouldlyOnDiskAssembly.GetName().Version!.Major;
        }

        if (assertionFramework == null || majorVersion == null)
        {
            throw new InvalidOperationException("Unable to determine assertion framework to use");
        }

        return (assertionFramework.Value, majorVersion.Value, assemblyLocation);
    }

    private static AssertionFramework GetDefaultAssertionFramework()
    {
        var factory = CreateAssertionsFactory();
        return factory.AssertionFramework;
    }

    private static string GetAssertionsAssemblyLocation()
    {
        var assemblyLocation = Path.GetDirectoryName(typeof(AssertionUtils).Assembly.Location);
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
            $"DragoAnt.Assertions.{assertionFramework:G}{majorVersion}.dll");

        if (!File.Exists(versionedLocation))
        {
            throw new InvalidOperationException(
                $"Detected {assertionFramework:G} version {majorVersion} but the assertions adapter wasn't found on disk");
        }

        var versionedAssembly = Assembly.LoadFrom(versionedLocation);

        return versionedAssembly.GetTypes()
            .SingleOrDefault(t => typeof(IPureAssertionsFactory).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);
    }

    private static bool IsFluentAssertionsAlreadyLoadedIntoDomain(
        [NotNullWhen(true)] out Assembly? fluentAssertionsAssembly)
    {
        fluentAssertionsAssembly = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(assembly =>
            {
                if (!assembly.GetName().Name!.Equals("FluentAssertions", StringComparison.Ordinal))
                {
                    return false;
                }

                if (IsAwesomeAssertionsAssembly(assembly))
                {
                    return false;
                }

                var metadataAttributes = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().ToArray();

                return !metadataAttributes.Any() ||
                       metadataAttributes.Any(metadata => metadata.Value.IndexOf("FluentAssertions", StringComparison.OrdinalIgnoreCase) >= 0);
            });

        return fluentAssertionsAssembly != null;
    }

    private static bool IsAwesomeAssertionsAlreadyLoadedIntoDomain(
        [NotNullWhen(true)] out Assembly? awesomeAssertionsAssembly)
    {
        awesomeAssertionsAssembly = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(IsAwesomeAssertionsAssembly);

        return awesomeAssertionsAssembly != null;
    }

    private static bool IsAwesomeAssertionsAssembly(Assembly assembly)
    {
        var name = assembly.GetName().Name;
        if (name.Equals("AwesomeAssertions", StringComparison.Ordinal))
        {
            return true;
        }

        if (!name.Equals("FluentAssertions", StringComparison.Ordinal))
        {
            return false;
        }

        if (assembly.GetCustomAttributes<AssemblyProductAttribute>()
            .Any(product =>
                product.Product.Equals("AwesomeAssertions", StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        return assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .Any(metadata => metadata.Value.IndexOf("AwesomeAssertions", StringComparison.OrdinalIgnoreCase) >= 0);
    }

    private static bool IsShouldlyAlreadyLoadedIntoDomain(
        [NotNullWhen(true)] out Assembly? shouldlyAssembly)
    {
        shouldlyAssembly = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(assembly => assembly.GetName().Name.Equals("Shouldly"));

        return shouldlyAssembly != null;
    }

    private static bool IsFluentAssertionsAvailableOnDisk(
        string assemblyLocation,
        [NotNullWhen(true)] out Assembly? assembly)
    {
        foreach (var baseDir in GetAssertionProbeDirectories(assemblyLocation))
        {
            var assemblyPath = Path.Combine(baseDir, "FluentAssertions.dll");
            if (!File.Exists(assemblyPath))
            {
                continue;
            }

            assembly = Assembly.LoadFrom(assemblyPath);
            if (IsAwesomeAssertionsAssembly(assembly))
            {
                continue;
            }

            var metadataAttributes = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().ToList();

            if (!metadataAttributes.Any() ||
                metadataAttributes.Any(metadata => metadata.Value.IndexOf("FluentAssertions", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                return true;
            }
        }

        assembly = null;
        return false;
    }

    private static bool IsAwesomeAssertionsAvailableOnDisk(
        string assemblyLocation,
        [NotNullWhen(true)] out Assembly? assembly)
    {
        foreach (var baseDir in GetAssertionProbeDirectories(assemblyLocation))
        {
            foreach (var fileName in new[] { "AwesomeAssertions.dll", "FluentAssertions.dll" })
            {
                var assemblyPath = Path.Combine(baseDir, fileName);
                if (!File.Exists(assemblyPath))
                {
                    continue;
                }

                assembly = Assembly.LoadFrom(assemblyPath);
                if (!IsAwesomeAssertionsAssembly(assembly))
                {
                    continue;
                }

                return true;
            }
        }

        assembly = null;
        return false;
    }

    private static bool IsShouldlyAvailableOnDisk(
        string assemblyLocation,
        [NotNullWhen(true)] out Assembly? assembly)
    {
        foreach (var baseDir in GetAssertionProbeDirectories(assemblyLocation))
        {
            var assemblyPath = Path.Combine(baseDir, "Shouldly.dll");

            if (File.Exists(assemblyPath))
            {
                assembly = Assembly.LoadFrom(assemblyPath);
                return true;
            }
        }

        assembly = null;
        return false;
    }

    /// <summary>
    /// NuGet consumers resolve assertion libraries next to the test assembly (BaseDirectory), not only
    /// next to <see cref="DragoAnt.Assertions"/> in the package lib folder.
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