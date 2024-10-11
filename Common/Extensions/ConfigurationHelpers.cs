using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.Extensions;

public static class ConfigurationHelpers
{
    public static T TryGetValue<T>(this IConfiguration configuration, string key, T defaultValue)
    {
        try
        {
            return configuration.GetValue(key, defaultValue) ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    public static void LogConfiguration(this  IConfiguration configuration, ILogger<dynamic> logger)
    {
        foreach (var section in configuration.GetChildren())
        {
            logger.LogWarning("######################### Configuration section: {SectionKey} #########################",
                section.Key);
            
            foreach (var child in section.GetChildren())
            {
                logger.LogWarning("Configuration child: {ChildKey}", child.Key);
            }
            
            logger.LogWarning("################################### End of section ###################################");
        }
    }
}