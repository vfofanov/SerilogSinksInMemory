namespace Serilog.Sinks.InMemory.AssertionsFrameworkExtension;

public partial class LogEventsPropertyAssertionImpl : LogEventsPropertyAssertion
{
    private readonly string _propertyName;
    private readonly LogEventsAssertions _logEventsAssertions;

    public LogEventsAssertions WithValues(params object?[] values)
    {
        Assert(Subject.Count == values.Length,
            $"Can't assert property values because {values.Length} values were provided while only {Subject.Count} messages were expected");

        var propertyValues = Subject
            .Select(logEvent => GetValueFromProperty(logEvent.Properties[_propertyName]))
            .ToArray();

        var notFound = values
            .Where(v => !propertyValues.Contains(v, PropertyValueEquality.Comparer))
            .ToArray();

        Assert(!notFound.Any(),
            new("Expected property values {0} to contain {1} but did not find {2}",
                propertyValues,
                values,
                notFound));

        return _logEventsAssertions;
    }

    private object? GetValueFromProperty(LogEventPropertyValue instance)
        => instance switch
        {
            ScalarValue scalarValue => scalarValue.Value,
            SequenceValue sequenceValue => sequenceValue.Elements.Select(e => e is ScalarValue sv ? sv.Value : e).ToArray(),
            _ => instance.ToString(),
        };
}
