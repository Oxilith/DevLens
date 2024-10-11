using Application.Extensions;
using Application.Interfaces;
using Application.Services;
using Application.Strategies;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Factories;

public static class CommitStrategyFactory
{
    public static ICommitStrategy CreateStrategy(ILogger<ChangeTrackingService> logger, IConfiguration configuration,
        ICommitRepository commitRepository)
    {
        var repositoryUriString = configuration.TryGetValue("RepositorySettings:RemoteRepositoryUri", String.Empty);
        var repositoryLocalPath = configuration.TryGetValue("RepositorySettings:LocalPath", String.Empty);
        
        if (string.IsNullOrEmpty(repositoryLocalPath) && string.IsNullOrEmpty(repositoryUriString))
        {
            throw new ArgumentException("Repository settings not found in configuration");
        }
        
        return string.IsNullOrEmpty(repositoryLocalPath) 
            ? new RemoteGitStrategy(commitRepository, new Uri(repositoryUriString!))
            : new LocalGitStrategy(commitRepository, repositoryLocalPath);
    }
}