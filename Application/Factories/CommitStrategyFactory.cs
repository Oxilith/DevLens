using Application.Extensions;
using Application.Interfaces;
using Application.Strategies;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Factories;

public static class CommitStrategyFactory
{
    public static ICommitStrategy CreateStrategy(IConfiguration configuration, ICommitRepository commitRepository)
    {
        var repositoryUriString = configuration.TryGetValue<string?>("RepositorySettings:RemoteRepositoryUri", null);
        var repositoryLocalPath = configuration.TryGetValue<string?>("RepositorySettings:LocalPath", null);

        if (repositoryLocalPath is null && repositoryUriString is null)
        {
            throw new ArgumentException("Repository settings not found in configuration");
        }
        
        return repositoryLocalPath == null
            ? new RemoteGitStrategy(commitRepository, new Uri(repositoryUriString!))
            : new LocalGitStrategy(commitRepository, repositoryLocalPath);
    }
}