namespace Humanizer;

using System.Globalization;

/// <summary>
/// Thin convenience wrapper around the strategies so the demo can call
/// something that looks like Humanizer's <c>.Humanize()</c> extension methods.
/// All real work is done by the copied DateTimeHumanizeStrategy classes.
/// </summary>
public static class HumanizeExtensions
{
    static readonly IDateTimeHumanizeStrategy defaultDateTime = new DefaultDateTimeHumanizeStrategy();
    static readonly IDateTimeOffsetHumanizeStrategy defaultDateTimeOffset = new DefaultDateTimeOffsetHumanizeStrategy();
    static readonly IDateOnlyHumanizeStrategy defaultDateOnly = new DefaultDateOnlyHumanizeStrategy();
    static readonly ITimeOnlyHumanizeStrategy defaultTimeOnly = new DefaultTimeOnlyHumanizeStrategy();

    public static string Humanize(this DateTime input, CultureInfo? culture = null) =>
        defaultDateTime.Humanize(input, DateTime.Now, culture);

    public static string Humanize(this DateTimeOffset input, CultureInfo? culture = null) =>
        defaultDateTimeOffset.Humanize(input, DateTimeOffset.UtcNow, culture);

    public static string Humanize(this DateOnly input, CultureInfo? culture = null) =>
        defaultDateOnly.Humanize(input, DateOnly.FromDateTime(DateTime.Today), culture);

    public static string Humanize(this TimeOnly input, CultureInfo? culture = null) =>
        defaultTimeOnly.Humanize(input, TimeOnly.FromDateTime(DateTime.Now), culture);

    /// <summary>
    /// Precision-based variant, e.g. <c>date.Humanize(0.85)</c>.
    /// </summary>
    public static string Humanize(this DateTime input, double precision, CultureInfo? culture = null) =>
        new PrecisionDateTimeHumanizeStrategy(precision).Humanize(input, DateTime.Now, culture);
}
