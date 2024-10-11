using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Strategies;

public class RemoteGitStrategy(ICommitRepository commitRepository, Uri repositoryUri) : ICommitStrategy
{
    public IReadOnlyCollection<Commit> GetCommits(int numberOfCommits)
    {
        return commitRepository.GetRemoteCommits(repositoryUri, numberOfCommits);
    }

    public string GetRepositoryPath()
    {
        return repositoryUri.AbsoluteUri;
    }
}