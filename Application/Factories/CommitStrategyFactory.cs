using Application.Interfaces;
using Application.Strategies;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Factories;

public static class CommitStrategyFactory
{
    public static ICommitStrategy CreateStrategy(IConfiguration configuration, ICommitRepository commitRepository,
        string? repositoryPath)
    {
        Uri defaultRepositoryUri =
            new(configuration["DefaultRepositoryUri"] ?? "https://github.com/microsoft/VFSForGit.git");
        return repositoryPath == null
            ? new RemoteGitStrategy(commitRepository, defaultRepositoryUri)
            : new LocalGitStrategy(commitRepository, repositoryPath);
    }
}