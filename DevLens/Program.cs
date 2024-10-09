using Application;
using DevLens.Components;
using Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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