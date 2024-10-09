namespace Domain.Entities;

public class ProjectChange
{
    public ProjectChange(DateTime changeDate, string commitMessage, List<ClassChange> classChanges)
    {
        ChangeDate = changeDate;
        CommitMessage = commitMessage;
        ClassChanges = classChanges;
    }

    public DateTime ChangeDate { get; private set; }
    public string CommitMessage { get; private set; }
    public List<ClassChange> ClassChanges { get; private set; }
}