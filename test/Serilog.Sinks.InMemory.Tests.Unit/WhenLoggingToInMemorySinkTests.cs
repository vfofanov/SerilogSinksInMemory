using FluentAssertions;
using Serilog.Core;
using Xunit;

namespace Serilog.Sinks.InMemory.Tests.Unit
{
    public class WhenLoggingToInMemorySinkTests
    {
        [Fact]
        public void GivenInformationMessageIsWritten_LogEventIsStoredInSink()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.InMemory()
                .CreateLogger();

            logger.Information("Test");

            InMemorySink.Instance
                .LogEvents
                .Should()
                .HaveCount(1);
        }

        [Fact]
        public void GivenRestrictedMinimumLevelMessagesAreWritten_FilteredLogEventsAreStoredInSink()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.InMemory(restrictedToMinimumLevel: Events.LogEventLevel.Information)
                .CreateLogger();

            logger.Verbose("Verbose Message");
            logger.Debug("Debug Message");
            logger.Information("Information Message");

            InMemorySink.Instance
                .LogEvents
                .Should()
                .HaveCount(1);
        }

        [Fact]
        public void GivenLoggerIsDisposed_LogEventsAreCleared()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.InMemory()
                .CreateLogger();

            logger.Information("Test");

            logger.Dispose();

            InMemorySink.Instance
                .LogEvents
                .Should()
                .HaveCount(0);
        }

        [Fact]
        public void GivenLoggerIsDisposedAndNewMessageIsLogged_SinkOnlyContainsSecondMessage()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.InMemory()
                .CreateLogger();

            logger.Information("First");

            logger.Dispose();

            logger.Information("Second");

            InMemorySink.Instance
                .LogEvents
                .Should()
                .OnlyContain(l => l.MessageTemplate.Text == "Second");
        }

        [Fact]
        public void GivenSnapshotIsTaken_SnapshotDoesNotIncludeEventsLoggedAfterSnapshot()
        {
            var sink = new InMemorySink();
            var logger = new LoggerConfiguration()
                .WriteTo.Sink(sink)
                .CreateLogger();

            logger.Information("First");
            logger.Information("Second");
            var snapshot = sink.Snapshot();
            logger.Information("Third");

            snapshot.LogEvents.Should().HaveCount(2);
            sink.LogEvents.Should().HaveCount(3);
        }

        [Fact]
        public void GivenSnapshotIsWrittenTo_EmitThrowsInvalidOperationException()
        {
            var sink = new InMemorySink();
            var logger = new LoggerConfiguration()
                .WriteTo.Sink(sink)
                .CreateLogger();

            logger.Information("First");

            var snapshot = sink.Snapshot();

            Action act = () => snapshot.Emit(snapshot.LogEvents.Single());

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Can't write log events to a in-memory sink snapshot because it is a read-only representation");
        }

        [Fact]
        public void GivenLevelSwitchChanges_OnlyEventsAtOrAboveCurrentLevelAreStored()
        {
            var sink = new InMemorySink();
            var levelSwitch = new LoggingLevelSwitch(Events.LogEventLevel.Information);
            var logger = new LoggerConfiguration()
                .WriteTo.InMemory(sink, levelSwitch: levelSwitch)
                .CreateLogger();

            logger.Information("Information before switch change");
            logger.Debug("Debug after initial level");

            levelSwitch.MinimumLevel = Events.LogEventLevel.Error;

            logger.Information("Information after switch change");
            logger.Error("Error after switch change");

            sink.LogEvents.Should().HaveCount(2);
            sink.LogEvents.Select(l => l.MessageTemplate.Text).Should().Equal(
                "Information before switch change",
                "Error after switch change");
        }

        [Fact]
        public void GivenMessagesAreWrittenConcurrently_AllLogEventsAreStored()
        {
            var sink = new InMemorySink();
            var logger = new LoggerConfiguration()
                .WriteTo.Sink(sink)
                .CreateLogger();

            const int numberOfMessages = 250;

            Parallel.For(0, numberOfMessages, i => logger.Information("Message {Number}", i));

            sink.LogEvents.Should().HaveCount(numberOfMessages);
        }

        [Fact]
        public void GivenDisposeIsCalledTwice_DoesNotThrow()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.InMemory()
                .CreateLogger();

            logger.Information("Test");

            Action act = () =>
            {
                logger.Dispose();
                logger.Dispose();
            };

            act.Should().NotThrow();
            InMemorySink.Instance.LogEvents.Should().BeEmpty();
        }
    }
}