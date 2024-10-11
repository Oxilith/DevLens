using Application.Interfaces;
using Application.Services;
using Application.Strategies;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Factories;

public static class CommitStrategyFactory
{
    public static ICommitStrategy CreateStrategy(ILogger<ChangeTrackingService> logger, IConfiguration configuration,
        ICommitRepository commitRepository)
    {
        var repositorySettings = new RepositorySettings(configuration);
      
        return !repositorySettings.UseLocalRepository
            ? new RemoteGitStrategy(commitRepository, new Uri(repositorySettings.RemoteRepositoryUri))
            : new LocalGitStrategy(commitRepository, repositorySettings.LocalPath);
    }
}