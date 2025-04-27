namespace AccessControlSystem.Application.Common.Utilities;

public static class RenewalCalculator
{
    public static string GetRenewalInfo(DateOnly endDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        if (endDate <= today) return "Expired";

        var (years, months, days) = CalculateRemainingTime(today, endDate);

        return FormatRenewalMessage(years, months, days);
    }

    private static string FormatRenewalMessage(int years, int months, int days)
    {
        var parts = new List<string>();

        parts.AddIf(years > 0, () => $"{years} year{Pluralize(years)}");
        parts.AddIf(months > 0, () => $"{months} month{Pluralize(months)}");
        parts.AddIf(days > 0 || parts.Count == 0, () => $"{days} day{Pluralize(days)}");

        return "Renewal in " + parts.JoinGrammatically();
    }

    private static (int years, int months, int days) CalculateRemainingTime(DateOnly fromDate, DateOnly toDate)
    {
        var years = CalculateYears(ref fromDate, toDate);
        var months = CalculateMonths(ref fromDate, toDate);
        var days = CalculateDays(fromDate, toDate);

        return (years, months, days);
    }

    private static int CalculateYears(ref DateOnly fromDate, DateOnly toDate)
    {
        var years = toDate.Year - fromDate.Year;
        var tempDate = fromDate.AddYears(years);

        if (tempDate <= toDate) return years;

        years--;
        fromDate = fromDate.AddYears(years);

        return years;
    }

    private static int CalculateMonths(ref DateOnly fromDate, DateOnly toDate)
    {
        var months = toDate.Month - fromDate.Month;

        if (months < 0)
        {
            months += 12;
        }

        var tempDate = fromDate.AddMonths(months);

        if (tempDate <= toDate) return months;

        months--;
        fromDate = fromDate.AddMonths(months);

        return months;
    }

    private static int CalculateDays(DateOnly fromDate, DateOnly toDate)
    {
        var days = toDate.Day - fromDate.Day;

        if (days < 0)
        {
            days += DateTime.DaysInMonth(fromDate.Year, fromDate.Month);
        }

        return days;
    }

    #region Helper Methods

    private static string Pluralize(int value) => value != 1 ? "s" : "";

    private static void AddIf(this List<string> list, bool condition, Func<string> itemFactory)
    {
        if (condition) list.Add(itemFactory());
    }

    private static string JoinGrammatically(this IReadOnlyList<string> parts)
    {
        return parts.Count switch
        {
            0 => string.Empty,
            1 => parts[0],
            2 => $"{parts[0]} and {parts[1]}",
            _ => $"{string.Join(", ", parts.Take(parts.Count - 1))} and {parts[^1]}"
        };
    }

    #endregion
}