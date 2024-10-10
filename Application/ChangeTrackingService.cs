using System.Collections.ObjectModel;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Application;

public class ChangeTrackingService(ICommitRepository commitRepository, IMemoryCache memoryCache)
    : IChangeTrackingService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const int NumberOfCommitsToFetch = 100 * 100 * 100 * 100;

    public IReadOnlyCollection<ProjectChange> GetChanges(string? repositoryPath = null)
    {
        var repoUri = repositoryPath == null ? new Uri("https://github.com/microsoft/VFSForGit.git") : null;
        repositoryPath ??= repoUri!.AbsoluteUri;

        if (_memoryCache.TryGetValue(repositoryPath, out IReadOnlyCollection<ProjectChange>? cachedChanges))
            return cachedChanges ?? new List<ProjectChange>().AsReadOnly();

        var commits = repoUri == null
            ? commitRepository.GetLocalCommits(repositoryPath, NumberOfCommitsToFetch)
            : commitRepository.GetRemoteCommits(new Uri(repositoryPath), NumberOfCommitsToFetch);

        var projectChanges = new ReadOnlyCollection<ProjectChange>(commits
            .Select(commit => new ProjectChange(commit.CommitDate, commit.Message, commit.ClassChanges.ToList()))
            .ToList());

        _memoryCache.Set(repositoryPath, projectChanges,
            new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });

        return projectChanges;
    }
}