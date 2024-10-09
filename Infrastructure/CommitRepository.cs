using System.Collections.ObjectModel;
using Domain;
using Domain.Entities;
using Infrastructure.Interfaces;
using LibGit2Sharp;
using Commit = Domain.Entities.Commit;

namespace Infrastructure;

public class CommitRepository : ICommitRepository
{
    public IReadOnlyCollection<Commit> GetCommits(string repositoryPath, int numberOfCommits)
    {
        using var repo = new Repository(repositoryPath);
        var commits = new List<Commit>();

        foreach (var commit in repo.Commits.Take(numberOfCommits))
        {
            var classChanges = new List<ClassChange>();

            var parent = commit.Parents.FirstOrDefault();
            if (parent != null)
            {
                var patch = repo.Diff.Compare<Patch>(parent.Tree, commit.Tree);
                classChanges.AddRange(patch.Select(change =>
                    new ClassChange(change.Path, change.Status.ToString(), change.Patch, commit.Author.When)));
            }

            commits.Add(new Commit(
                commit.Sha,
                commit.MessageShort,
                commit.Author.Name,
                commit.Author.When.DateTime,
                classChanges
            ));
        }

        return new ReadOnlyCollection<Commit>(commits);
    }
}