using Domain.Entities;

namespace Application.Interfaces;

public interface ICommitStrategy
{
    IReadOnlyCollection<Commit> GetCommits(int numberOfCommits);
    string GetRepositoryPath();
}