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
        //TODO: Clean up logging later. This is just for debugging purposes
        configuration.LogConfiguration(logger);
        
        var repositoryUriString = configuration.TryGetValue<string?>("RepositorySettings:RemoteRepositoryUri", null);
        logger.LogWarning("Repository URI: {RepositoryUri}", repositoryUriString ?? "Null");
        
        var repositoryLocalPath = configuration.TryGetValue<string?>("RepositorySettings:LocalPath", null);
        logger.LogWarning("Repository local path: {RepositoryLocalPath}", repositoryLocalPath ?? "Null");
        
        if (repositoryLocalPath is null && repositoryUriString is null)
        {
            throw new ArgumentException("Repository settings not found in configuration");
        }
        
        return repositoryLocalPath == null
            ? new RemoteGitStrategy(commitRepository, new Uri(repositoryUriString!))
            : new LocalGitStrategy(commitRepository, repositoryLocalPath);
    }
}