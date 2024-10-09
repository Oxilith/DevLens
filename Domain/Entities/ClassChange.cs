namespace Domain.Entities;

public class ClassChange(string className, string changeType, string diff, DateTimeOffset changeDate)
{
    public string ClassName { get; private set; } = className;
    public DateTimeOffset ChangeDate { get; private set; } = changeDate;
    public string ChangeType { get; private set; } = changeType;
    public string Diff { get; private set; } = diff;
}