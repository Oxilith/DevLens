using System.Collections.ObjectModel;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Application;

public class ChangeTrackingService(ICommitRepository commitRepository, IMemoryCache memoryCache) : IChangeTrackingService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const int NumberOfCommitsToFetch = 100 * 100 * 100 * 100;

    public IReadOnlyCollection<ProjectChange> GetChanges(string repositoryPath)
    {
        if (_memoryCache.TryGetValue(repositoryPath, out IReadOnlyCollection<ProjectChange>? cachedChanges))
        {
            return cachedChanges ?? new List<ProjectChange>().AsReadOnly();
        }

        var commits = commitRepository.GetCommits(repositoryPath, NumberOfCommitsToFetch);

        var projectChanges = new ReadOnlyCollection<ProjectChange>(commits
            .Select(commit => new ProjectChange(commit.CommitDate, commit.Message, commit.ClassChanges.ToList()))
            .ToList());
        
        _memoryCache.Set(repositoryPath, projectChanges, new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });
        
        return projectChanges;
    }
}