using Application;
using Application.Interfaces;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using DevLens;
using DevLens.Components;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.ApplicationInsights.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMemoryCache();

#if !DEBUG

builder.Services.AddOpenTelemetry().UseAzureMonitor(options =>
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"]);
builder.Services.AddApplicationInsightsTelemetry(options => options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"]);
builder.Services.AddSingleton<ITelemetryProcessorFactory>(sp => new DependencyFilterProcessorFactory());

#endif

builder.Services.AddScoped<ICommitRepository, CommitRepository>();
builder.Services.AddScoped<IChangeTrackingService, ChangeTrackingService>();

var repositoryPath = builder.Configuration.GetValue<string>("RepositorySettings:Path");
builder.Services.AddCascadingValue("Changes",
    p => p.GetRequiredService<IChangeTrackingService>()
        .GetChanges(repositoryPath ?? throw new InvalidOperationException("Repository path is not set")));

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