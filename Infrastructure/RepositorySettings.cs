using Common.Extensions;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class RepositorySettings
{
    public bool UseLocalRepository { get; }
    public string RemoteRepositoryUri { get; }
    public string LocalPath { get; }

    public RepositorySettings(IConfiguration configuration)
    {
        UseLocalRepository = configuration.TryGetValue<bool?>("RepositorySettings:UseLocalRepository", null)
                             ?? throw new ArgumentException("Repository settings not found in configuration");
        RemoteRepositoryUri = configuration.TryGetValue("RepositorySettings:RemoteRepositoryUri", string.Empty);
        LocalPath = configuration.TryGetValue("RepositorySettings:LocalPath", string.Empty); 
        
        ValidateConfiguration();
    }

    private void ValidateConfiguration()
    {
        if (string.IsNullOrEmpty(LocalPath) && UseLocalRepository)
        {
            throw new ArgumentException("Local repository path not found in configuration");
        }

        if (string.IsNullOrEmpty(RemoteRepositoryUri) && !UseLocalRepository)
        {
            throw new ArgumentException("Remote repository URI not found in configuration");
        }
    }
}