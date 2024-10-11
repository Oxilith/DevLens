using Domain.Entities;

namespace Application.Interfaces;

public interface IChangeTrackingService
{
    public IReadOnlyCollection<ProjectChange> GetChanges();
}