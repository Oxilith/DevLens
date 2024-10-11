using System.Collections.ObjectModel;
using System.Diagnostics;
using Domain.Entities;
using Infrastructure.Interfaces;
using LibGit2Sharp;
using Commit = Domain.Entities.Commit;

namespace Infrastructure;

public class CommitRepository : ICommitRepository
{
    public IReadOnlyCollection<Commit> GetLocalCommits(string repositoryPath, int numberOfCommits)
    {
        try
        {
            using var repo = new Repository(repositoryPath);
            return GetCommits(numberOfCommits, repo);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
    }

    public IReadOnlyCollection<Commit> GetRemoteCommits(Uri repositoryUri, int numberOfCommits)
    {
        var tempRoot = AppDomain.CurrentDomain.BaseDirectory;
        var tempDirectoryPath = Path.Combine(tempRoot, "Repository_" + Guid.NewGuid());

        try
        {
            Repository.Clone(repositoryUri.AbsoluteUri, tempDirectoryPath);

            using var repo = new Repository(tempDirectoryPath);
            return GetCommits(numberOfCommits, repo);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
        finally
        {
            Directory.Delete(tempDirectoryPath, true);
        }
    }

    private static IReadOnlyCollection<Commit> GetCommits(int numberOfCommits, Repository repo)
    {
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