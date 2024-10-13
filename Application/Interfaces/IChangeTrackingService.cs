using System.Collections.ObjectModel;
using Domain.Entities;

namespace Application.Interfaces;

public interface IChangeTrackingService
{
    public Task<ReadOnlyCollection<ProjectChange>> GetChangesAsync(CancellationToken cancellationToken);
}