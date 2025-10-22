#nullable enable

using System;
using System.Linq;
using Serilog.Events;
using Serilog.Sinks.InMemory.Assertions;
using Shouldly;

namespace Serilog.Sinks.InMemory.Shouldly4
{
    [ShouldlyMethods]
    public class InMemorySinkAssertionsImpl : InMemorySinkAssertions
    {
        public InMemorySinkAssertionsImpl(InMemorySink snapshotInstance)
        {
            Subject = snapshotInstance;
        }

        public InMemorySink Subject { get; }

        public LogEventsAssertions HaveMessage(
            Func<LogEvent, bool> predicate,
            string? predicateErrorName = null,
            string because = "",
            params object[] becauseArgs)
        {
            predicateErrorName ??= "<predicate>";

            var matches = Subject
                .LogEvents
                .Where(predicate)
                .ToArray();

            if (!matches.Any())
            {
                throw new ShouldAssertException($"Expected message \"{predicateErrorName}\" to be logged");
            }

            return new LogEventsAssertionsImpl(predicateErrorName, matches);
        }

        public LogEventsAssertions HaveMessage(
            string messageTemplate,
            string because = "",
            params object[] becauseArgs)
        {
            return HaveMessage(logEvent => logEvent.MessageTemplate.Text == messageTemplate, messageTemplate, because, becauseArgs);
        }

        public PatternLogEventsAssertions HaveMessage()
        {
            return new PatternLogEventsAssertionsImpl(Subject.LogEvents);
        }

        public void NotHaveMessage(
            string? messageTemplate = null,
            string because = "",
            params object[] becauseArgs)
        {
            if (messageTemplate != null)
            {
                NotHaveMessage(logEvent => logEvent.MessageTemplate.Text == messageTemplate, messageTemplate, because, becauseArgs);
            }
            else
            {
                NotHaveMessage(null, messageTemplate, because, becauseArgs);
            }
        }

        public void NotHaveMessage(Func<LogEvent, bool>? predicate, string? predicateErrorName = null, string because = "", params object[] becauseArgs)
        {
            predicateErrorName ??= "<predicate>";

            int count;
            string failureMessage;

            if (predicate != null)
            {
                count = Subject
                    .LogEvents
                    .Count(predicate);

                failureMessage = $"Expected message \"{predicateErrorName}\" not to be logged, but it was found {(count > 1 ? $"{count} times" : "once")}";
            }
            else
            {
                count = Subject
                    .LogEvents
                    .Count();

                failureMessage = $"Expected no messages to be logged, but found {(count > 1 ? $"{count} messages" : "message")}";
            }

            if (count != 0)
            {
                throw new ShouldAssertException(failureMessage);
            }
        }
    }
}