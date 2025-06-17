# Metalify Playlist API

A microservice API for managing playlists in the Metalify heavy metal music catalog built with ASP.NET Core and Entity Framework.

## Features

- **Independent Microservice Architecture**: Self-contained playlist management service that doesn't depend on external services for core functionality
- **Data Denormalization**: Stores song metadata locally for better performance and service independence
- **Entity Framework Core**: Data access with in-memory database for development, SQL Server for production
- **REST Best Practices**: RESTful endpoints following HTTP conventions
- **OpenAPI/Swagger**: Interactive API documentation
- **Health Checks**: Built-in health monitoring endpoints
- **CORS Support**: Configured for cross-origin requests
- **Microservices Best Practices**: Follows proper service boundaries and data ownership patterns

## Architecture

```text
Extensions/          # API endpoint mappings
├── PlaylistEndpoints.cs

Services/            # Business logic layer
├── IPlaylistService.cs
└── PlaylistService.cs

Repositories/        # Data access layer
├── IPlaylistRepository.cs
└── PlaylistRepository.cs

Models/              # Domain entities
├── Playlist.cs
├── PlaylistItem.cs (with denormalized song metadata)
└── SongReference.cs

DTOs/               # Data transfer objects
└── PlaylistDto.cs

Data/               # Entity Framework context
└── PlaylistDbContext.cs
```

## API Endpoints

### Playlists
- `GET /api/playlists` - Get all playlists
- `GET /api/playlists/{id}` - Get playlist by ID
- `GET /api/playlists/user/{userId}` - Get playlists by user
- `GET /api/playlists/public` - Get public playlists
- `GET /api/playlists/search?q={query}` - Search playlists by name or description
- `POST /api/playlists` - Create new playlist
- `PUT /api/playlists/{id}` - Update playlist
- `DELETE /api/playlists/{id}` - Delete playlist

### Playlist Management
- `POST /api/playlists/{id}/songs` - Add song to playlist
- `DELETE /api/playlists/{id}/songs/{songId}` - Remove song from playlist
- `PUT /api/playlists/{id}/reorder` - Reorder songs in playlist

### Health
- `GET /health` - Health check endpoint

## Getting Started

### Prerequisites
- .NET 9.0 SDK

### Running the API

1. **Navigate to the project directory:**
   ```bash
   cd Metalify.Playlist.Api
   ```

2. **Run the application:**
   ```bash
   dotnet run
   ```

3. **Access the API:**
   - Swagger UI: https://localhost:7228
   - API Base URL: https://localhost:7228/api
   - Health Check: https://localhost:7228/health

## Configuration

### Database
The API uses Entity Framework Core with:
- **Development**: In-memory database for quick testing
- **Production**: SQL Server (configure connection string in appsettings.json)

### Service Independence
This microservice operates independently and does not require external service dependencies. Song metadata is provided by the calling application when songs are added to playlists.

## Sample Data

The API includes seeded data for development:
- Test playlist with sample metadata

## Technologies Used

- **ASP.NET Core 9.0** - Web framework
- **Entity Framework Core** - ORM for data access
- **Swagger/OpenAPI** - API documentation
- **HttpClient** - External API communication
- **In-Memory Database** - Development database
- **SQL Server** - Production database

## CORS Support

CORS is configured to allow all origins in development. For production, configure specific origins in the CORS policy.

## Error Handling

The API includes comprehensive error handling:
- HTTP status codes following REST conventions
- Detailed error messages for debugging
- Graceful handling of external API failures

## Logging

Structured logging is configured with different levels:
- **Information**: General application flow
- **Warning**: Unexpected situations
- **Error**: Error conditions
- **Debug**: Detailed debugging information (Development only)

## Microservice Architecture

This service follows microservices best practices by:

- **Service Independence**: Operates without external service dependencies for core functionality
- **Data Ownership**: Stores denormalized song metadata locally for better performance
- **Fault Tolerance**: Continues to function even if other services are unavailable
- **Proper Boundaries**: Clear separation of concerns with other services
- **Eventual Consistency**: Accepts song metadata at playlist creation time

The service receives song metadata from the main application when songs are added to playlists, eliminating the need for cross-service calls during normal operations.

## Development

### Testing the API
Use the included `playlist-api-tests.http` file with VS Code REST Client extension or similar tools to test all endpoints.

### Adding New Features
1. Add new DTOs in the `DTOs` folder
2. Extend repository interfaces and implementations
3. Add business logic to the service layer
4. Map new endpoints in the `Extensions` folder
5. Update this README with new endpoints

## Deployment

For production deployment:
1. Configure SQL Server connection string
2. Set appropriate CORS origins
3. Configure catalog API base URL
4. Set up health check monitoring
5. Configure logging for production environment
