using Microsoft.Extensions.Configuration;

namespace Application.Extensions;

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
}