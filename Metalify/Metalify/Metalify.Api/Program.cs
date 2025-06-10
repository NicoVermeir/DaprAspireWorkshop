using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Metalify.Api.Data;
using Metalify.Api.Repositories;
using Metalify.Api.Services;
using Metalify.Api.DTOs;

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

// Minimal API Endpoints

#region Artist Endpoints

app.MapGet("/api/artists", async (IArtistService artistService) =>
{
    try
    {
        var artists = await artistService.GetAllArtistsAsync();
        return Results.Ok(artists);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while retrieving artists");
    }
})
.WithName("GetAllArtists")
.WithSummary("Get all artists")
.WithDescription("Retrieves a list of all artists in the catalog")
.Produces<IEnumerable<ArtistSummaryDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Artists");

app.MapGet("/api/artists/{id:guid}", async (Guid id, IArtistService artistService) =>
{
    try
    {
        if (id == Guid.Empty)
        {
            return Results.BadRequest("Artist ID cannot be empty");
        }

        var artist = await artistService.GetArtistByIdAsync(id);
        
        if (artist == null)
        {
            return Results.NotFound($"Artist with ID {id} not found");
        }

        return Results.Ok(artist);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while retrieving the artist");
    }
})
.WithName("GetArtistById")
.WithSummary("Get artist by ID")
.WithDescription("Retrieves an artist by their unique identifier, including their albums")
.Produces<ArtistDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Artists");

app.MapGet("/api/artists/search", async (string q, IArtistService artistService) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Results.BadRequest("Search query cannot be empty");
        }

        var artists = await artistService.SearchArtistsAsync(q);
        return Results.Ok(artists);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while searching artists");
    }
})
.WithName("SearchArtists")
.WithSummary("Search artists")
.WithDescription("Search for artists by name using a query string")
.Produces<IEnumerable<ArtistSummaryDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Artists");

app.MapGet("/api/artists/country/{country}", async (string country, IArtistService artistService) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            return Results.BadRequest("Country cannot be empty");
        }

        var artists = await artistService.GetArtistsByCountryAsync(country);
        return Results.Ok(artists);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while retrieving artists by country");
    }
})
.WithName("GetArtistsByCountry")
.WithSummary("Get artists by country")
.WithDescription("Retrieves all artists from a specific country")
.Produces<IEnumerable<ArtistSummaryDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Artists");

#endregion

#region Album Endpoints

app.MapGet("/api/albums", async (IAlbumService albumService) =>
{
    try
    {
        var albums = await albumService.GetAllAlbumsAsync();
        return Results.Ok(albums);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while retrieving albums");
    }
})
.WithName("GetAllAlbums")
.WithSummary("Get all albums")
.WithDescription("Retrieves a list of all albums in the catalog")
.Produces<IEnumerable<AlbumSummaryDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Albums");

app.MapGet("/api/albums/{id:guid}", async (Guid id, IAlbumService albumService) =>
{
    try
    {
        if (id == Guid.Empty)
        {
            return Results.BadRequest("Album ID cannot be empty");
        }

        var album = await albumService.GetAlbumByIdAsync(id);
        
        if (album == null)
        {
            return Results.NotFound($"Album with ID {id} not found");
        }

        return Results.Ok(album);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while retrieving the album");
    }
})
.WithName("GetAlbumById")
.WithSummary("Get album by ID")
.WithDescription("Retrieves an album by its unique identifier, including its songs")
.Produces<AlbumDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Albums");

app.MapGet("/api/albums/artist/{artistId:guid}", async (Guid artistId, IAlbumService albumService) =>
{
    try
    {
        if (artistId == Guid.Empty)
        {
            return Results.BadRequest("Artist ID cannot be empty");
        }

        var albums = await albumService.GetAlbumsByArtistIdAsync(artistId);
        return Results.Ok(albums);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while retrieving albums by artist");
    }
})
.WithName("GetAlbumsByArtist")
.WithSummary("Get albums by artist")
.WithDescription("Retrieves all albums by a specific artist")
.Produces<IEnumerable<AlbumSummaryDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Albums");

app.MapGet("/api/albums/year/{year:int}", async (int year, IAlbumService albumService) =>
{
    try
    {
        if (year < 1900 || year > DateTime.Now.Year + 1)
        {
            return Results.BadRequest("Year must be between 1900 and current year + 1");
        }

        var albums = await albumService.GetAlbumsByYearAsync(year);
        return Results.Ok(albums);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while retrieving albums by year");
    }
})
.WithName("GetAlbumsByYear")
.WithSummary("Get albums by year")
.WithDescription("Retrieves all albums released in a specific year")
.Produces<IEnumerable<AlbumSummaryDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Albums");

app.MapGet("/api/albums/search", async (string q, IAlbumService albumService) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Results.BadRequest("Search query cannot be empty");
        }

        var albums = await albumService.SearchAlbumsAsync(q);
        return Results.Ok(albums);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while searching albums");
    }
})
.WithName("SearchAlbums")
.WithSummary("Search albums")
.WithDescription("Search for albums by title using a query string")
.Produces<IEnumerable<AlbumSummaryDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Albums");

#endregion

#region Song Endpoints

app.MapGet("/api/songs", async (ISongService songService) =>
{
    try
    {
        var songs = await songService.GetAllSongsAsync();
        return Results.Ok(songs);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while retrieving songs");
    }
})
.WithName("GetAllSongs")
.WithSummary("Get all songs")
.WithDescription("Retrieves a list of all songs in the catalog")
.Produces<IEnumerable<SongDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Songs");

app.MapGet("/api/songs/{id:guid}", async (Guid id, ISongService songService) =>
{
    try
    {
        if (id == Guid.Empty)
        {
            return Results.BadRequest("Song ID cannot be empty");
        }

        var song = await songService.GetSongByIdAsync(id);
        
        if (song == null)
        {
            return Results.NotFound($"Song with ID {id} not found");
        }

        return Results.Ok(song);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while retrieving the song");
    }
})
.WithName("GetSongById")
.WithSummary("Get song by ID")
.WithDescription("Retrieves a song by its unique identifier")
.Produces<SongDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Songs");

app.MapGet("/api/songs/album/{albumId:guid}", async (Guid albumId, ISongService songService) =>
{
    try
    {
        if (albumId == Guid.Empty)
        {
            return Results.BadRequest("Album ID cannot be empty");
        }

        var songs = await songService.GetSongsByAlbumIdAsync(albumId);
        return Results.Ok(songs);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while retrieving songs by album");
    }
})
.WithName("GetSongsByAlbum")
.WithSummary("Get songs by album")
.WithDescription("Retrieves all songs from a specific album")
.Produces<IEnumerable<SongDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Songs");

app.MapGet("/api/songs/artist/{artistId:guid}", async (Guid artistId, ISongService songService) =>
{
    try
    {
        if (artistId == Guid.Empty)
        {
            return Results.BadRequest("Artist ID cannot be empty");
        }

        var songs = await songService.GetSongsByArtistIdAsync(artistId);
        return Results.Ok(songs);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while retrieving songs by artist");
    }
})
.WithName("GetSongsByArtist")
.WithSummary("Get songs by artist")
.WithDescription("Retrieves all songs by a specific artist")
.Produces<IEnumerable<SongDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Songs");

app.MapGet("/api/songs/search", async (string q, ISongService songService) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Results.BadRequest("Search query cannot be empty");
        }

        var songs = await songService.SearchSongsAsync(q);
        return Results.Ok(songs);
    }
    catch (Exception)
    {
        return Results.Problem("An error occurred while searching songs");
    }
})
.WithName("SearchSongs")
.WithSummary("Search songs")
.WithDescription("Search for songs by title using a query string")
.Produces<IEnumerable<SongDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.WithTags("Songs");

#endregion

// Add a health check endpoint
app.MapGet("/health", () => new { Status = "Healthy", Timestamp = DateTime.UtcNow })
    .WithName("HealthCheck")
    .WithSummary("Health check")
    .WithDescription("Returns the health status of the API")
    .Produces(StatusCodes.Status200OK)
    .WithTags("Health");

app.Run();
