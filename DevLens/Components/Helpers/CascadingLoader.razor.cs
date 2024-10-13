using System.Collections.ObjectModel;
using Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace DevLens.Components.Helpers;

public partial class CascadingLoader: ComponentBase
{
    // [CascadingParameter(Name = nameof(WaitFor))] public TDependency? WaitFor { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    private bool Loaded => !Loading;
    private bool Loading { get;  set; } = true;
    private bool ShowErrorMessages { get; set; } = false;
    [Parameter] public string? Dependency { get; set; } = null;
    [CascadingParameter] public IDictionary<string, object>? CascadingParameters { get; set; }

    protected override async Task OnInitializedAsync()
    {

        var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));

            try
            {
                while (!Loaded && !cts.Token.IsCancellationRequested)
                {
                    // Check if the cascading parameter with the name equal to Dependency is available
                    if (CascadingParameters != null && Dependency != null && CascadingParameters.TryGetValue(Dependency, out _))
                    {
                        Console.WriteLine($"Dependency '{Dependency}' resolved.");
                        Loading = false;
                    }
                    else
                    {
                        Console.WriteLine($"Waiting for dependency '{Dependency}'...");
                    }

                    await Task.Delay(1000, cts.Token);
                    await InvokeAsync(StateHasChanged);
                }
            }
            catch (TaskCanceledException)
            {
                ShowErrorMessages = true;
                Loading = false;
            }

            if (!Loaded)
            {
                ShowErrorMessages = true;
            }

            await InvokeAsync(StateHasChanged);

        await base.OnInitializedAsync();
    }

}
