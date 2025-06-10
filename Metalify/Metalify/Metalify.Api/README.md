# Metalify Catalog API

A REST API for the Metalify heavy metal music catalog built with ASP.NET Core and Entity Framework.

## Features

- **N-tier Architecture**: Clean separation of concerns with Controllers → Services → Repositories → Data layers
- **Entity Framework Core**: Data access with in-memory database for development, SQL Server for production
- **REST Best Practices**: RESTful endpoints following HTTP conventions
- **OpenAPI/Swagger**: Interactive API documentation
- **Seeded Data**: Preloaded with classic metal bands, albums, and songs inspired by Metal Archives

## Architecture

```
Controllers/         # REST API endpoints
├── ArtistsController.cs
├── AlbumsController.cs
└── SongsController.cs

Services/            # Business logic layer
├── IArtistService.cs
├── IAlbumService.cs
├── ISongService.cs
└── Implementation files

Repositories/        # Data access layer
├── IArtistRepository.cs
├── IAlbumRepository.cs
├── ISongRepository.cs
└── Implementation files

Models/              # Domain entities
├── Artist.cs
├── Album.cs
└── Song.cs

DTOs/               # Data transfer objects
├── ArtistDto.cs
├── AlbumDto.cs
└── SongDto.cs

Data/               # Entity Framework context
└── MetalifyCatalogDbContext.cs
```

## API Endpoints

### Artists
- `GET /api/artists` - Get all artists
- `GET /api/artists/{id}` - Get artist by ID with albums
- `GET /api/artists/search?q={query}` - Search artists by name
- `GET /api/artists/country/{country}` - Get artists by country

### Albums
- `GET /api/albums` - Get all albums
- `GET /api/albums/{id}` - Get album by ID with songs
- `GET /api/albums/artist/{artistId}` - Get albums by artist
- `GET /api/albums/year/{year}` - Get albums by release year
- `GET /api/albums/search?q={query}` - Search albums by title

### Songs
- `GET /api/songs` - Get all songs
- `GET /api/songs/{id}` - Get song by ID
- `GET /api/songs/album/{albumId}` - Get songs by album
- `GET /api/songs/artist/{artistId}` - Get songs by artist
- `GET /api/songs/search?q={query}` - Search songs by title

### Health
- `GET /health` - Health check endpoint

## Getting Started

1. **Run the API**:
   ```bash
   cd Metalify.Api
   dotnet run
   ```

2. **Access Swagger UI**: Navigate to `http://localhost:5101` (configured to serve Swagger at root)

3. **Test Endpoints**: Use the interactive Swagger UI or tools like curl/Postman

## Sample Data

The API comes pre-seeded with classic metal bands:
- **Iron Maiden** (UK, 1975) - Albums: The Number of the Beast, Powerslave
- **Metallica** (US, 1981) - Albums: Master of Puppets, Ride the Lightning  
- **Black Sabbath** (UK, 1968) - Albums: Paranoid
- **Judas Priest** (UK, 1969)
- **Megadeth** (US, 1983)

## Configuration

### Development
- Uses Entity Framework In-Memory database
- Seeded with sample data
- CORS enabled for Blazor client

### Production
- Configure SQL Server connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=MetalifyCatalog;..."
  }
}
```

## Technologies Used

- **ASP.NET Core 9.0** - Web API framework
- **Entity Framework Core 9.0** - ORM and data access
- **Swagger/OpenAPI** - API documentation
- **In-Memory Database** - Development data store
- **SQL Server** - Production database (configurable)

## CORS Support

The API includes CORS configuration to allow requests from the Blazor frontend. In production, configure specific origins for security.

## Error Handling

All endpoints include proper HTTP status codes:
- `200 OK` - Successful requests
- `400 Bad Request` - Invalid input
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server errors

## Logging

Comprehensive logging throughout all layers using built-in ASP.NET Core logging with configurable levels.
