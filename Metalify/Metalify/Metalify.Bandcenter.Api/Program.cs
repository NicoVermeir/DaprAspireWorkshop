using Microsoft.EntityFrameworkCore;
using Metalify.Bandcenter.Api.Data;
using Metalify.Bandcenter.Api.Extensions;
using Metalify.Bandcenter.Api.Repositories;
using Metalify.Bandcenter.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "Metalify Catalog API", 
        Version = "v1",
        Description = "A comprehensive catalog API for managing metal bands, albums, and songs"
    });
});

// Add Entity Framework
builder.Services.AddDbContext<CatalogDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Use in-memory database for development
        options.UseInMemoryDatabase("CatalogDb");
    }
    else
    {
        // Use SQL Server for production
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        options.UseSqlServer(connectionString);
    }
});

// Register repositories
builder.Services.AddScoped<IBandRepository, BandRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();

// Register services
builder.Services.AddScoped<IBandService, BandService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<ISongService, SongService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Metalify Catalog API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });

    // Ensure database is created and seeded
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        context.Database.EnsureCreated();
    }
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

// Map API endpoints
app.MapBandEndpoints();
app.MapAlbumEndpoints();
app.MapSongEndpoints();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
   .WithName("HealthCheck")
   .WithSummary("Health check endpoint")
   .WithDescription("Returns the health status of the API");

app.Run();
