namespace Domain.Entities;

public class ProjectChange(DateTime changeDate, string commitMessage, IReadOnlyCollection<ClassChange> classChanges)
{
    public DateTime ChangeDate { get; private set; } = changeDate;
    public string CommitMessage { get; private set; } = commitMessage;
    public IReadOnlyCollection<ClassChange> ClassChanges { get; private set; } = classChanges;
}