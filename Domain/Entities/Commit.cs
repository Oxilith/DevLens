namespace Domain.Entities;

public class Commit(string id, string message, string author, DateTime commitDate, List<ClassChange> classChanges)
{
    public string Id { get; private set; } = id;
    public string Message { get; private set; } = message;
    public string Author { get; private set; } = author;
    public DateTime CommitDate { get; private set; } = commitDate;
    public IReadOnlyList<ClassChange> ClassChanges { get; private set; } = classChanges;
}