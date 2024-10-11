using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Strategies;

public class LocalGitStrategy(ICommitRepository commitRepository, string repositoryPath) : ICommitStrategy
{
    public IReadOnlyCollection<Commit> GetCommits(int numberOfCommits)
    {
        return commitRepository.GetLocalCommits(repositoryPath, numberOfCommits);
    }

    public string GetRepositoryPath()
    {
        return repositoryPath;
    }
}