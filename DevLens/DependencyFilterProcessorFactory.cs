using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.ApplicationInsights.Extensibility;

namespace DevLens;

public class DependencyFilterProcessorFactory : ITelemetryProcessorFactory
{
    public ITelemetryProcessor Create(ITelemetryProcessor next)
    {
        return new DependencyFilterProcessor(next);
    }
}