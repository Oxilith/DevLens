using Domain.ValueObjects;

namespace Domain.Models;

public class MonthlyChange
{
    private MonthlyChange(YearMonth month, int changeCount)
    {
        Month = month;
        ChangeCount = changeCount;
    }

    public YearMonth Month { get; private set; }
    public int ChangeCount { get; private set; }

    public static MonthlyChange Default(YearMonth month)
    {
        return new MonthlyChange(month, 0);
    }

    public static MonthlyChange New(YearMonth month, int changeCount)
    {
        return new MonthlyChange(month, changeCount);
    }
}