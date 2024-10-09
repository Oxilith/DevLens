using Domain;
using Domain.Entities;

namespace Infrastructure;

public interface ICommitRepository
{
    IEnumerable<Commit> GetCommits(string repositoryPath, int numberOfCommits);
}