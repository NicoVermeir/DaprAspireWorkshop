using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Metalify.Catalog.Api.Data;
using Metalify.Catalog.Api.Repositories;
using Metalify.Catalog.Api.Services;
using Metalify.Catalog.Api.DTOs;
using Metalify.Catalog.Api.Extensions;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (minimal APIs don't need AddControllers)
builder.Services.AddEndpointsApiExplorer();

// Configure Entity Framework with In-Memory database for development
// In production, you would configure SQL Server connection string
builder.Services.AddDbContext<MetalifyCatalogDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseInMemoryDatabase("MetalifyCatalog");
    }
    else
    {
        // Use connection string from appsettings for production
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(connectionString);
    }
});

// Register repositories
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();

// Register services
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<ISongService, SongService>();

// Configure OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Metalify Catalog API",
        Version = "v1",
        Description = "A REST API for the Metalify heavy metal music catalog",
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
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MetalifyCatalogDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Metalify Catalog API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient");
app.UseCloudEvents();

// Map API Endpoints
app.MapSubscriptionEndpoints();
app.MapArtistEndpoints();
app.MapAlbumEndpoints();
app.MapSongEndpoints();
app.MapHealthEndpoints();
app.MapSubscribeHandler();

app.Run();
