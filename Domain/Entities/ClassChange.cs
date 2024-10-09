namespace Domain.Entities;

public class ClassChange
{
    public ClassChange(string className, string changeType, string diff, DateTimeOffset changeDate)
    {
        ClassName = className;
        ChangeType = changeType;
        Diff = diff;
        ChangeDate = changeDate;
    }

    public string ClassName { get; private set; }
    public DateTimeOffset ChangeDate { get; private set; }
    public string ChangeType { get; private set; }
    public string Diff { get; private set; }
}