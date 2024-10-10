using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ICommitRepository
{
    IReadOnlyCollection<Commit> GetLocalCommits(string repositoryPath, int numberOfCommits);
    IReadOnlyCollection<Commit> GetRemoteCommits(Uri? repositoryUri, int numberOfCommits);
}