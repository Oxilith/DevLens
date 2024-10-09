using Domain;
using Domain.Entities;
using Infrastructure;

namespace Application;

public class ChangeTrackingService : IChangeTrackingService
{
    private readonly ICommitRepository _commitRepository;

    public ChangeTrackingService(ICommitRepository commitRepository)
    {
        _commitRepository = commitRepository;
    }

    public List<ProjectChange> GetChanges(string repositoryPath)
    {
        var commits = _commitRepository.GetCommits(repositoryPath, 100000); // Example: last 100000 commits

        return commits
            .Select(commit => new ProjectChange(commit.CommitDate, commit.Message, commit.ClassChanges.ToList()))
            .ToList();
    }
}