using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace DevLens;

public class DependencyFilterProcessor(ITelemetryProcessor next) : ITelemetryProcessor
{
    public void Process(ITelemetry item)
    {
        // Exclude DependencyTelemetry logs
        if (!(item is DependencyTelemetry)) next.Process(item);
    }
}