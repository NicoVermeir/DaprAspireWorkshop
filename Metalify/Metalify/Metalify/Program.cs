using Metalify.Components;
using Metalify.Services;
using Metalify.Services.Interfaces;
using Serilog;
using Serilog.Events;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/metalify-.log",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Warning)
    .CreateLogger();

try
{
    Log.Information("Starting Metalify application");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog to the application
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.File(
            path: "logs/metalify-.log",
            rollingInterval: RollingInterval.Day,
            restrictedToMinimumLevel: context.HostingEnvironment.IsDevelopment()
                ? LogEventLevel.Information
                : LogEventLevel.Warning));

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents()
        .AddInteractiveWebAssemblyComponents();

    // Configure HTTP client for API communication
    string catalogApiUrl = builder.Configuration["MetalifyApi:CatalogApiUrl"] ?? string.Empty;
    builder.Services.AddHttpClient<ICatalogService, CatalogService>(client =>
    {
        client.BaseAddress = new Uri(catalogApiUrl);
    });

    string playlistApiUrl = builder.Configuration["MetalifyApi:PlaylistApiUrl"] ?? string.Empty;
    builder.Services.AddHttpClient<IPlaylistService, PlaylistService>(client =>
    {
        client.BaseAddress = new Uri(playlistApiUrl);
    });

    // Register Music Services (for server-side rendering)
    builder.Services.AddScoped<ISearchService, SearchService>();
    builder.Services.AddSingleton<IAudioPlayerService, AudioPlayerService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(typeof(Metalify.Client._Imports).Assembly);

    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
