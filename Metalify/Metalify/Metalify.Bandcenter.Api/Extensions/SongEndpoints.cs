using Microsoft.AspNetCore.Mvc;
using Metalify.Bandcenter.Api.DTOs;
using Metalify.Bandcenter.Api.Services;

namespace Metalify.Bandcenter.Api.Extensions;

public static class SongEndpoints
{
    public static void MapSongEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/songs")
            .WithTags("Songs")
            .WithOpenApi();

        // GET /api/songs
        group.MapGet("/", async (ISongService songService) =>
        {
            var songs = await songService.GetAllSongsAsync();
            return Results.Ok(songs);
        })
        .WithName("GetAllSongs")
        .WithSummary("Get all songs")
        .WithDescription("Retrieves a list of all songs in the catalog");        // GET /api/songs/{id}
        group.MapGet("/{id:guid}", async (Guid id, ISongService songService) =>
        {
            var song = await songService.GetSongByIdAsync(id);
            return song == null ? Results.NotFound($"Song with ID {id} not found") : Results.Ok(song);
        })
        .WithName("GetSongById")
        .WithSummary("Get song by ID")
        .WithDescription("Retrieves a specific song by its ID");

        // GET /api/songs/album/{albumId}
        group.MapGet("/album/{albumId:guid}", async (Guid albumId, ISongService songService) =>
        {
            var songs = await songService.GetSongsByAlbumIdAsync(albumId);
            return Results.Ok(songs);
        })
        .WithName("GetSongsByAlbum")
        .WithSummary("Get songs by album")
        .WithDescription("Retrieves all songs from a specific album");

        // GET /api/songs/band/{bandId}
        group.MapGet("/band/{bandId:guid}", async (Guid bandId, ISongService songService) =>
        {
            var songs = await songService.GetSongsByBandIdAsync(bandId);
            return Results.Ok(songs);
        })
        .WithName("GetSongsByBand")
        .WithSummary("Get songs by band")
        .WithDescription("Retrieves all songs by a specific band");

        // GET /api/songs/search?title={searchTerm}
        group.MapGet("/search", async ([FromQuery] string title, ISongService songService) =>
        {
            if (string.IsNullOrWhiteSpace(title))
                return Results.BadRequest("Search term 'title' is required");

            var songs = await songService.SearchSongsByTitleAsync(title);
            return Results.Ok(songs);
        })
        .WithName("SearchSongsByTitle")
        .WithSummary("Search songs by title")
        .WithDescription("Searches for songs by title containing the specified term");        // GET /api/songs/genre/{genre}
        group.MapGet("/genre/{genre}", async (string genre, ISongService songService) =>
        {
            var songs = await songService.SearchSongsByTitleAsync(genre);
            return Results.Ok(songs);
        })
        .WithName("GetSongsByGenre")
        .WithSummary("Get songs by genre")
        .WithDescription("Retrieves songs that match the specified genre");

        // GET /api/songs/duration?min={minDuration}&max={maxDuration}
        group.MapGet("/duration", async ([FromQuery] string min, [FromQuery] string max, ISongService songService) =>
        {
            if (string.IsNullOrWhiteSpace(min) || string.IsNullOrWhiteSpace(max))
                return Results.BadRequest("Both min and max duration parameters are required (format: HH:MM:SS)");

            if (!TimeSpan.TryParse(min, out var minDuration) || !TimeSpan.TryParse(max, out var maxDuration))
                return Results.BadRequest("Invalid duration format. Use HH:MM:SS format");

            if (minDuration > maxDuration)
                return Results.BadRequest("Minimum duration cannot be greater than maximum duration");

            var songs = await songService.GetSongsByDurationRangeAsync(minDuration, maxDuration);
            return Results.Ok(songs);
        })
        .WithName("GetSongsByDurationRange")
        .WithSummary("Get songs by duration range")
        .WithDescription("Retrieves songs within the specified duration range");

        // POST /api/songs
        group.MapPost("/", async ([FromBody] CreateSongDto createSongDto, ISongService songService) =>
        {
            try
            {
                var song = await songService.CreateSongAsync(createSongDto);
                return Results.CreatedAtRoute("GetSongById", new { id = song.Id }, song);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CreateSong")
        .WithSummary("Create a new song")
        .WithDescription("Creates a new song in the catalog");        // PUT /api/songs/{id}
        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateSongDto updateSongDto, ISongService songService) =>
        {
            try
            {
                var song = await songService.UpdateSongAsync(id, updateSongDto);
                return song == null ? Results.NotFound($"Song with ID {id} not found") : Results.Ok(song);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("UpdateSong")
        .WithSummary("Update a song")
        .WithDescription("Updates an existing song in the catalog");

        // DELETE /api/songs/{id}
        group.MapDelete("/{id:guid}", async (Guid id, ISongService songService) =>
        {
            var deleted = await songService.DeleteSongAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound($"Song with ID {id} not found");
        })
        .WithName("DeleteSong")
        .WithSummary("Delete a song")
        .WithDescription("Deletes a song from the catalog");
    }
}
