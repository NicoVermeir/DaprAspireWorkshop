using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Metalify.Client.Services;
using Metalify.Client.Services.Interfaces;
using Serilog;
using Serilog.Core;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configure Serilog for the client
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.BrowserConsole()
    .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
    loggingBuilder.AddSerilog(dispose: true));

// Add services for our application
builder.Services.AddScoped<HttpClient>();

// Register our services
builder.Services.AddScoped<IMusicDataService, FakeMusicDataService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddSingleton<IAudioPlayerService, AudioPlayerService>();

await builder.Build().RunAsync();
