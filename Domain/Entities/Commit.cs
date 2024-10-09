namespace Domain.Entities;

public class Commit
{
    public Commit(string id, string message, string author, DateTime commitDate, List<ClassChange> classChanges)
    {
        Id = id;
        Message = message;
        Author = author;
        CommitDate = commitDate;
        ClassChanges = classChanges;
    }

    public string Id { get; private set; }
    public string Message { get; private set; }
    public string Author { get; private set; }
    public DateTime CommitDate { get; private set; }
    public IReadOnlyList<ClassChange> ClassChanges { get; private set; }
}