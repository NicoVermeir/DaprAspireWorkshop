using Metalify.Catalog.Api.Services;
using Metalify.Catalog.Api.DTOs;

namespace Metalify.Catalog.Api.Extensions;

public static class SongEndpoints
{
    public static void MapSongEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/songs")
            .WithTags("Songs");

        group.MapGet("", async (ISongService songService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("{id:guid}", async (Guid id, ISongService songService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("album/{albumId:guid}", async (Guid albumId, ISongService songService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("artist/{artistId:guid}", async (Guid artistId, ISongService songService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("search", async (string q, ISongService songService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
