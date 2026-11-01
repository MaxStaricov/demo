namespace Humanizer;

using System.Globalization;

/// <summary>
/// Minimal replacement for Humanizer's localisation layer.
/// The real library pulls in hundreds of resources; this demo only needs the
/// strategy classes from src/Humanizer/DateTimeHumanizeStrategy, so a tiny
/// English formatter is plugged in here instead.
/// </summary>
public interface IFormatter
{
    string DateHumanize(TimeUnit timeUnit, Tense tense, int dateTimeOffset);
}

/// <summary>
/// Simple English formatter ("2 days ago", "in 3 hours", ...).
/// </summary>
public class DefaultFormatter : IFormatter
{
    public static readonly DefaultFormatter Instance = new();

    DefaultFormatter()
    {
    }

    public string DateHumanize(TimeUnit timeUnit, Tense tense, int dateTimeOffset)
    {
        if (timeUnit == TimeUnit.Millisecond && dateTimeOffset == 0)
        {
            return tense == Tense.Past ? "now" : "in a moment";
        }

        var unit = Pluralize(timeUnit, dateTimeOffset);

        return tense == Tense.Past
            ? $"{dateTimeOffset} {unit} ago"
            : $"in {dateTimeOffset} {unit}";
    }

    /// <summary>
    /// Used by the DateOnly strategies when input equals comparisonBase.
    /// </summary>
    public string DateHumanize_Today(string now) => "today";

    static string Pluralize(TimeUnit unit, int count) =>
        count == 1 ? Singular(unit) : Singular(unit) + "s";

    static string Singular(TimeUnit unit) =>
        unit switch
        {
            TimeUnit.Millisecond => "millisecond",
            TimeUnit.Second => "second",
            TimeUnit.Minute => "minute",
            TimeUnit.Hour => "hour",
            TimeUnit.Day => "day",
            TimeUnit.Week => "week",
            TimeUnit.Month => "month",
            TimeUnit.Year => "year",
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
}

/// <summary>
/// Replacement for Humanizer's Configurator: exposes the same
/// <c>GetFormatter(CultureInfo?)</c> entry point used by the strategies.
/// </summary>
public static class Configurator
{
    public static IFormatter GetFormatter(CultureInfo? culture) => DefaultFormatter.Instance;
}
