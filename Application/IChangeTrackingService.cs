using Domain;
using Domain.Entities;

namespace Application;

public interface IChangeTrackingService
{
    public List<ProjectChange> GetChanges(string repositoryPath);
}