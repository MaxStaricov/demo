using System.Globalization;
using Humanizer;

// Демонстрация подсистемы DateTimeHumanizeStrategy из Humanizer.
// Исходники скопированы из https://github.com/Humanizr/Humanizer/tree/main/src/Humanizer/DateTimeHumanizeStrategy
// Локализация (форматтер) заменена минимальным английским шимом — см. src/.../Shim/HumanizerShim.cs.

var now = DateTime.Now;

Section("DefaultDateTimeHumanizeStrategy — DateTime.Humanize()");
Console.WriteLine($"  {Offset(now, -350, TimeUnit.Millisecond):O} -> {Offset(now, -350, TimeUnit.Millisecond).Humanize()}");
Console.WriteLine($"  {Offset(now, -25, TimeUnit.Second):O} -> {Offset(now, -25, TimeUnit.Second).Humanize()}");
Console.WriteLine($"  {Offset(now, -90, TimeUnit.Second):O} -> {Offset(now, -90, TimeUnit.Second).Humanize()}");
Console.WriteLine($"  {Offset(now, -14, TimeUnit.Minute):O} -> {Offset(now, -14, TimeUnit.Minute).Humanize()}");
Console.WriteLine($"  {Offset(now, -75, TimeUnit.Minute):O} -> {Offset(now, -75, TimeUnit.Minute).Humanize()}");
Console.WriteLine($"  {Offset(now, -5, TimeUnit.Hour):O} -> {Offset(now, -5, TimeUnit.Hour).Humanize()}");
Console.WriteLine($"  {Offset(now, 26, TimeUnit.Hour):O} -> {Offset(now, 26, TimeUnit.Hour).Humanize()}");
Console.WriteLine($"  {Offset(now, -2, TimeUnit.Day):O} -> {Offset(now, -2, TimeUnit.Day).Humanize()}");
Console.WriteLine($"  {Offset(now, -12, TimeUnit.Day):O} -> {Offset(now, -12, TimeUnit.Day).Humanize()}");
Console.WriteLine($"  {Offset(now, -9, TimeUnit.Day):O} -> {Offset(now, -9, TimeUnit.Day).Humanize()}");
Console.WriteLine($"  {Offset(now, -3, TimeUnit.Month):O} -> {Offset(now, -3, TimeUnit.Month).Humanize()}");
Console.WriteLine($"  {Offset(now, 2, TimeUnit.Year):O} -> {Offset(now, 2, TimeUnit.Year).Humanize()}");
Console.WriteLine($"  {now:O} -> {now.Humanize()}");

Section("PrecisionDateTimeHumanizeStrategy — приближение с точностью (precision)");
var past = Offset(now, -45, TimeUnit.Minute);
Console.WriteLine($"  {past:O}");
Console.WriteLine($"    precision 0.75 (default) -> {past.Humanize()} | strategy -> {new PrecisionDateTimeHumanizeStrategy().Humanize(past, now, null)}");
Console.WriteLine($"    precision 0.85           -> {new PrecisionDateTimeHumanizeStrategy(0.85).Humanize(past, now, null)}");
Console.WriteLine($"    precision 0.50           -> {new PrecisionDateTimeHumanizeStrategy(0.5).Humanize(past, now, null)}");
Console.WriteLine($"  {Offset(now, -20, TimeUnit.Day):O} -> precision 0.75 -> {new PrecisionDateTimeHumanizeStrategy().Humanize(Offset(now, -20, TimeUnit.Day), now, null)}");
Console.WriteLine($"  {Offset(now, -400, TimeUnit.Day):O} -> precision 0.75 -> {new PrecisionDateTimeHumanizeStrategy().Humanize(Offset(now, -400, TimeUnit.Day), now, null)}");

Section("Своё сравнение с базовой датой (без расширений)");
var releaseDay = new DateTime(2026, 10, 1, 9, 0, 0);
var reportDay = new DateTime(2026, 11, 1, 9, 0, 0);
var strategy = new DefaultDateTimeHumanizeStrategy();
Console.WriteLine($"  релиз относительно отчёта -> {strategy.Humanize(releaseDay, reportDay, null)}");
Console.WriteLine($"  отчёт относительно релиза -> {strategy.Humanize(reportDay, releaseDay, null)}");

Section("DateOnly / TimeOnly / DateTimeOffset стратегии");
Console.WriteLine($"  DateOnly  today            -> {DateOnly.FromDateTime(DateTime.Today).Humanize()}");
Console.WriteLine($"  DateOnly  +1 day           -> {DateOnly.FromDateTime(DateTime.Today).AddDays(1).Humanize()}");
Console.WriteLine($"  DateOnly  -400 days        -> {DateOnly.FromDateTime(DateTime.Today).AddDays(-400).Humanize()}");
Console.WriteLine($"  TimeOnly  now              -> {TimeOnly.FromDateTime(DateTime.Now).Humanize()}");
Console.WriteLine($"  TimeOnly  +45 min          -> {TimeOnly.FromDateTime(DateTime.Now).AddMinutes(45).Humanize()}");
Console.WriteLine($"  DateTimeOffset -2 hours    -> {DateTimeOffset.UtcNow.AddHours(-2).Humanize()}");

Section("Custom strategy — свой IDateTimeHumanizeStrategy");
var custom = new RussianishStrategy();
Console.WriteLine($"  {Offset(now, -2, TimeUnit.Day):O} -> {custom.Humanize(Offset(now, -2, TimeUnit.Day), now, CultureInfo.InvariantCulture)}");

Console.WriteLine();
Console.WriteLine("Done.");

static void Section(string title)
{
    Console.WriteLine();
    Console.WriteLine(title);
    Console.WriteLine(new string('-', title.Length));
}

static DateTime Offset(DateTime basis, int amount, TimeUnit unit) =>
    unit switch
    {
        TimeUnit.Millisecond => basis.AddMilliseconds(amount),
        TimeUnit.Second => basis.AddSeconds(amount),
        TimeUnit.Minute => basis.AddMinutes(amount),
        TimeUnit.Hour => basis.AddHours(amount),
        TimeUnit.Day => basis.AddDays(amount),
        TimeUnit.Week => basis.AddDays(amount * 7),
        TimeUnit.Month => basis.AddMonths(amount),
        TimeUnit.Year => basis.AddYears(amount),
        _ => throw new ArgumentOutOfRangeException(nameof(unit))
    };

/// <summary>
/// Демонстрирует расширяемость подсистемы: любой класс, реализующий
/// IDateTimeHumanizeStrategy, можно подать вместо дефолтного.
/// </summary>
sealed class RussianishStrategy : IDateTimeHumanizeStrategy
{
    public string Humanize(DateTime input, DateTime comparisonBase, CultureInfo? culture)
    {
        var ts = new TimeSpan(Math.Abs(comparisonBase.Ticks - input.Ticks));
        var future = input > comparisonBase;

        var (unit, count) = ts.TotalDays >= 365 ? ("г.", (int)(ts.TotalDays / 365))
            : ts.TotalDays >= 30 ? ("мес.", (int)(ts.TotalDays / 30))
            : ts.TotalDays >= 1 ? ("дн.", (int)ts.TotalDays)
            : ts.TotalHours >= 1 ? ("ч.", (int)ts.TotalHours)
            : ts.TotalMinutes >= 1 ? ("мин.", (int)ts.TotalMinutes)
            : ("сек.", Math.Max(1, (int)ts.TotalSeconds));

        return future ? $"через {count} {unit}" : $"{count} {unit} назад";
    }
}
