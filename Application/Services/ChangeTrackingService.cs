using Application.Factories;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ChangeTrackingService(
    ICommitRepository commitRepository,
    IMemoryCache memoryCache,
    IConfiguration configuration,
    int numberOfCommitsToFetch = 1000000)
    : IChangeTrackingService
{
    public IReadOnlyCollection<ProjectChange> GetChanges()
    {
        var strategy = CommitStrategyFactory.CreateStrategy(configuration, commitRepository);

        if (memoryCache.TryGetValue(strategy.GetRepositoryPath(),
                out IReadOnlyCollection<ProjectChange>? cachedChanges))
            return cachedChanges ?? new List<ProjectChange>().AsReadOnly();

        var projectChanges = strategy
            .GetCommits(numberOfCommitsToFetch)
            .Select(commit => new ProjectChange(commit.CommitDate, commit.Message, commit.ClassChanges))
            .ToList()
            .AsReadOnly();

        memoryCache.Set(strategy.GetRepositoryPath(),
            projectChanges,
            new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });

        return projectChanges;
    }
}