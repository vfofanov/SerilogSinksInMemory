using System;
using System.Reflection;
using FluentAssertions;
using Serilog.Configuration;
using Serilog.Core;
using Xunit;

namespace Serilog.Sinks.InMemory.Tests.Unit
{
    public class WhenConfiguringInMemorySinkTests
    {
        [Fact]
        public void GivenConfigurationToWriteToInMemorySink_InMemorySinkIsAddedToLogger()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.InMemory()
                .CreateLogger();

            var sinks = GetConfiguredSinks(logger);

            sinks.Should().Contain(s => s.GetType() == typeof(InMemorySink));
        }

        [Fact]
        public void GivenNullSinkConfiguration_InMemoryThrowsArgumentNullException()
        {
            LoggerSinkConfiguration? sinkConfiguration = null;

            Action act = () => sinkConfiguration!.InMemory();

            var exception = act.Should().Throw<ArgumentNullException>().Which;
            exception.ParamName.Should().Be("sinkConfiguration");
        }

        [Fact]
        public void GivenConfigurationToWriteToExplicitInMemorySink_ProvidedSinkIsAddedToLogger()
        {
            var sink = new InMemorySink();
            var logger = new LoggerConfiguration()
                .WriteTo.InMemory(sink)
                .CreateLogger();

            var sinks = GetConfiguredSinks(logger);

            sinks.Should().Contain(s => ReferenceEquals(s, sink));
        }

        [Fact]
        public void GivenNullSink_InMemoryThrowsArgumentNullException()
        {
            var loggerConfiguration = new LoggerConfiguration();

            Action act = () => loggerConfiguration.WriteTo.InMemory(null);

            var exception = act.Should().Throw<ArgumentNullException>().Which;
            exception.ParamName.Should().Be("sink");
        }

        private static ILogEventSink[] GetConfiguredSinks(ILogger logger)
        {
            // Because there is no way to get access to the sinks configured for a logger
            // we need to use reflection to get at it...

            // The first one is a SafeAggregatedSink which is internal
            var instance = logger
                .GetType()
                .GetField("_sink", BindingFlags.Instance | BindingFlags.NonPublic)!
                .GetValue(logger);

            // It has a field containing all configured sinks
            return (ILogEventSink[])instance!.GetType()
                .GetField("_sinks", BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(instance)!;
        }
    }
}