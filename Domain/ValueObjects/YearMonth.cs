namespace Domain.ValueObjects;

public readonly struct YearMonth : IComparable<YearMonth>
{
    public int Year { get; }
    public int Month { get; }

    private YearMonth(int year, int month)
    {
        Year = year;
        Month = month;
    }

    public static YearMonth Default => new(2010, 1);

    public static YearMonth New(int year, int month)
    {
        return new YearMonth(year, month);
    }

    public YearMonth NextMonth()
    {
        return Month == 12 ? New(Year + 1, 1) : New(Year, Month + 1);
    }

    public int CompareTo(YearMonth other)
    {
        var yearComparison = Year.CompareTo(other.Year);
        return yearComparison != 0 ? yearComparison : Month.CompareTo(other.Month);
    }

    public override string ToString()
    {
        return $"{Year:D4}-{Month:D2}";
    }
}