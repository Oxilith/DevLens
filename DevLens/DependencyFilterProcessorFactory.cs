using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.ApplicationInsights.Extensibility;

namespace DevLens;

public class DependencyFilterProcessorFactory(bool shouldFilterDependencyLogs) : ITelemetryProcessorFactory
{
    public ITelemetryProcessor Create(ITelemetryProcessor next)
    {
        return shouldFilterDependencyLogs ? new DependencyFilterProcessor(next) : next;
    }
}