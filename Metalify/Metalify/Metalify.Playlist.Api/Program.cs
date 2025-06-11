using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Metalify.Playlist.Api.Data;
using Metalify.Playlist.Api.Repositories;
using Metalify.Playlist.Api.Services;
using Metalify.Playlist.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();

// Configure Entity Framework with In-Memory database for development
// In production, you would configure SQL Server connection string
builder.Services.AddDbContext<PlaylistDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseInMemoryDatabase("MetalifyPlaylist");
    }
    else
    {
        // Use connection string from appsettings for production
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(connectionString);
    }
});

// Register repositories
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();

// Register services
builder.Services.AddScoped<IPlaylistService, PlaylistService>();

// Register HTTP client for catalog API
builder.Services.AddHttpClient<ICatalogApiService, CatalogApiService>(client =>
{
    // Configure the base address for the catalog API
    var catalogApiUrl = builder.Configuration.GetValue<string>("CatalogApi:BaseUrl") ?? "https://localhost:7001";
    client.BaseAddress = new Uri(catalogApiUrl);
    client.DefaultRequestHeaders.Add("User-Agent", "Metalify-Playlist-Service/1.0");
});

// Configure OpenAPI/Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Metalify Playlist API",
        Version = "v1",
        Description = "A microservice API for managing playlists in the Metalify heavy metal music catalog",
        Contact = new OpenApiContact
        {
            Name = "Metalify Team",
            Email = "dev@metalify.com"
        }
    });

    // Include XML comments for better API documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configure CORS for development (adjust for production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<PlaylistDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Metalify Playlist API v1");
        c.RoutePrefix = string.Empty; // Makes Swagger UI available at the app root
    });
    
    // Seed database in development
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<PlaylistDbContext>();
        await context.Database.EnsureCreatedAsync();
        await context.SeedDataAsync();
    }
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

// Map API endpoints
app.MapPlaylistEndpoints();

// Map health check endpoint
app.MapHealthChecks("/health");

// Add a simple root endpoint
app.MapGet("/", () => "Metalify Playlist API - Heavy Metal Playlist Management Service")
    .WithName("GetRoot")
    .WithSummary("API Root")
    .WithDescription("Welcome endpoint for the Metalify Playlist API")
    .Produces<string>(StatusCodes.Status200OK);

app.Run();
