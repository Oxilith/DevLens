namespace Domain.Entities;

public class ProjectChange(DateTime changeDate, string commitMessage, List<ClassChange> classChanges)
{
    public DateTime ChangeDate { get; private set; } = changeDate;
    public string CommitMessage { get; private set; } = commitMessage;
    public List<ClassChange> ClassChanges { get; private set; } = classChanges;
}