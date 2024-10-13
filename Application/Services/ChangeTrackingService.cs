using System.Collections.ObjectModel;
using Application.Factories;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ChangeTrackingService(
    ILogger<ChangeTrackingService> logger,
    ICommitRepository commitRepository,
    IMemoryCache memoryCache,
    IConfiguration configuration,
    int numberOfCommitsToFetch = 10000)
    : IChangeTrackingService
{
    public async Task<ReadOnlyCollection<ProjectChange>> GetChangesAsync(CancellationToken token)
    {
        return await Task.Run(async () =>
        {
            var strategy = CommitStrategyFactory.CreateStrategy(logger, configuration, commitRepository);

            if (memoryCache.TryGetValue(strategy.GetRepositoryPath(),
                    out ReadOnlyCollection<ProjectChange>? cachedChanges))
                return await Task.FromResult(cachedChanges ?? new List<ProjectChange>().AsReadOnly());

            var projectChanges = strategy
                .GetCommits(numberOfCommitsToFetch)
                .Select(commit => new ProjectChange(commit.CommitDate, commit.Message, commit.ClassChanges))
                .ToList()
                .AsReadOnly();

            memoryCache.Set(strategy.GetRepositoryPath(),
                projectChanges,
                new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });

            return await Task.FromResult(projectChanges);
        }, token);
    }
}