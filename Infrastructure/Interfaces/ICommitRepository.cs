using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ICommitRepository
{
    IReadOnlyCollection<Commit> GetCommits(string repositoryPath, int numberOfCommits);
}