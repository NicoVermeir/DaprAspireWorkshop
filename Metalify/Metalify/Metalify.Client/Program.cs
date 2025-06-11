using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
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

await builder.Build().RunAsync();