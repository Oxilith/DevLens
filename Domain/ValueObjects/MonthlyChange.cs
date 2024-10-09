namespace Domain.ValueObjects;

public class MonthlyChange : IComparable<MonthlyChange>
{
    private MonthlyChange(YearMonth month, int changeCount)
    {
        Month = month;
        ChangeCount = changeCount;
    }

    public YearMonth Month { get; init; }
    public int ChangeCount { get; init; }

    public static MonthlyChange Default(YearMonth month)
    {
        return new MonthlyChange(month, 0);
    }

    public static MonthlyChange New(YearMonth month, int changeCount)
    {
        return new MonthlyChange(month, changeCount);
    }

    public int CompareTo(MonthlyChange? other)
    {
        var yearMonthComparison = Month.CompareTo(other?.Month ?? YearMonth.Default);
        return yearMonthComparison != 0 ? yearMonthComparison : ChangeCount.CompareTo(other?.ChangeCount ?? 0);
    }
}