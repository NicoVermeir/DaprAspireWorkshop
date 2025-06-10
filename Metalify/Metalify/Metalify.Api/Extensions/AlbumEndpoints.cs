using Metalify.Api.Services;
using Metalify.Api.DTOs;

namespace Metalify.Api.Extensions;

public static class AlbumEndpoints
{
    public static void MapAlbumEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/albums")
            .WithTags("Albums");

        group.MapGet("", async (IAlbumService albumService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("{id:guid}", async (Guid id, IAlbumService albumService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("artist/{artistId:guid}", async (Guid artistId, IAlbumService albumService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("year/{year:int}", async (int year, IAlbumService albumService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("search", async (string q, IAlbumService albumService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
