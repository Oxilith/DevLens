using Application.Extensions;
using Application.Interfaces;
using Application.Services;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using DevLens;
using DevLens.Components;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.ApplicationInsights.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<ICommitRepository, CommitRepository>();
builder.Services.AddScoped<IChangeTrackingService, ChangeTrackingService>();

if (builder.Environment.IsDevelopment())
{
    var appInsightsConnectionString =
        builder.Configuration.TryGetValue<string?>("ApplicationInsights:ConnectionString", default)
        ?? throw new InvalidOperationException("Application Insights connection string is not set");

    builder.Services.AddApplicationInsightsTelemetry(options => options.ConnectionString = appInsightsConnectionString);
    builder.Services.AddOpenTelemetry()
        .UseAzureMonitor(options => options.ConnectionString = appInsightsConnectionString);

    // In development, we want to use a local git repository
    builder.Services.AddCascadingValue("Changes",
        p => p
            .GetRequiredService<IChangeTrackingService>()
            .GetChanges());

}
else
{
    builder.Services.AddOpenTelemetry().UseAzureMonitor();
    builder.Services.AddApplicationInsightsTelemetry();

    // In production, we want to use a remote git repository
    builder.Services.AddCascadingValue("Changes",
        p => p.GetRequiredService<IChangeTrackingService>()
            .GetChanges());
}

builder.Services
    .AddSingleton<ITelemetryProcessorFactory>(_ => new DependencyFilterProcessorFactory(
        builder.Configuration.TryGetValue("FeatureToggles:EnableAppInsightsDependencyAnalysis", false)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();