using Metalify.Playlist.Api.Services;
using Metalify.Playlist.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Metalify.Playlist.Api.Extensions;

/// <summary>
/// Extension methods for mapping playlist endpoints
/// </summary>
public static class PlaylistEndpoints
{
    public static void MapPlaylistEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/playlists")
            .WithTags("Playlists")
            .WithOpenApi();

        // GET /api/playlists - Get all playlists
        group.MapGet("/", async (IPlaylistService playlistService) =>
        {
            try
            {
                var playlists = await playlistService.GetAllPlaylistsAsync();
                return Results.Ok(playlists);
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while retrieving playlists");
            }
        })
        .WithName("GetAllPlaylists")
        .WithSummary("Get all playlists")
        .WithDescription("Retrieve all playlists from the system")
        .Produces<IEnumerable<PlaylistSummaryDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        // GET /api/playlists/{id} - Get playlist by ID
        group.MapGet("/{id:guid}", async (Guid id, IPlaylistService playlistService) =>
        {
            try
            {
                var playlist = await playlistService.GetPlaylistByIdAsync(id);
                return playlist != null ? Results.Ok(playlist) : Results.NotFound();
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while retrieving the playlist");
            }
        })
        .WithName("GetPlaylistById")
        .WithSummary("Get playlist by ID")
        .WithDescription("Retrieve a specific playlist by its unique identifier")
        .Produces<PlaylistDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        // GET /api/playlists/user/{userId} - Get playlists by user
        group.MapGet("/user/{userId}", async (string userId, IPlaylistService playlistService) =>
        {
            try
            {
                var playlists = await playlistService.GetPlaylistsByUserAsync(userId);
                return Results.Ok(playlists);
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while retrieving user playlists");
            }
        })
        .WithName("GetPlaylistsByUser")
        .WithSummary("Get playlists by user")
        .WithDescription("Retrieve all playlists created by a specific user")
        .Produces<IEnumerable<PlaylistSummaryDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        // GET /api/playlists/public - Get public playlists
        group.MapGet("/public", async (IPlaylistService playlistService) =>
        {
            try
            {
                var playlists = await playlistService.GetPublicPlaylistsAsync();
                return Results.Ok(playlists);
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while retrieving public playlists");
            }
        })
        .WithName("GetPublicPlaylists")
        .WithSummary("Get public playlists")
        .WithDescription("Retrieve all public playlists")
        .Produces<IEnumerable<PlaylistSummaryDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        // GET /api/playlists/search?q={query} - Search playlists
        group.MapGet("/search", async ([FromQuery] string q, IPlaylistService playlistService) =>
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                {
                    return Results.BadRequest("Search query cannot be empty");
                }

                var playlists = await playlistService.SearchPlaylistsAsync(q);
                return Results.Ok(playlists);
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while searching playlists");
            }
        })
        .WithName("SearchPlaylists")
        .WithSummary("Search playlists")
        .WithDescription("Search for playlists by name or description using a query string")
        .Produces<IEnumerable<PlaylistSummaryDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);

        // POST /api/playlists - Create new playlist
        group.MapPost("/", async ([FromBody] CreatePlaylistDto createDto, IPlaylistService playlistService) =>
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createDto.Name))
                {
                    return Results.BadRequest("Playlist name is required");
                }

                var playlist = await playlistService.CreatePlaylistAsync(createDto);
                return Results.Created($"/api/playlists/{playlist.Id}", playlist);
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while creating the playlist");
            }
        })
        .WithName("CreatePlaylist")
        .WithSummary("Create new playlist")
        .WithDescription("Create a new playlist with the provided details")
        .Produces<PlaylistDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);

        // PUT /api/playlists/{id} - Update playlist
        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdatePlaylistDto updateDto, IPlaylistService playlistService) =>
        {
            try
            {
                var playlist = await playlistService.UpdatePlaylistAsync(id, updateDto);
                return playlist != null ? Results.Ok(playlist) : Results.NotFound();
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while updating the playlist");
            }
        })
        .WithName("UpdatePlaylist")
        .WithSummary("Update playlist")
        .WithDescription("Update an existing playlist with new details")
        .Produces<PlaylistDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        // DELETE /api/playlists/{id} - Delete playlist
        group.MapDelete("/{id:guid}", async (Guid id, IPlaylistService playlistService) =>
        {
            try
            {
                var success = await playlistService.DeletePlaylistAsync(id);
                return success ? Results.NoContent() : Results.NotFound();
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while deleting the playlist");
            }
        })
        .WithName("DeletePlaylist")
        .WithSummary("Delete playlist")
        .WithDescription("Delete a playlist by its unique identifier")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        // POST /api/playlists/{id}/songs - Add song to playlist
        group.MapPost("/{id:guid}/songs", async (Guid id, [FromBody] AddSongToPlaylistDto addSongDto, IPlaylistService playlistService) =>
        {
            try
            {
                var playlist = await playlistService.AddSongToPlaylistAsync(id, addSongDto);
                return playlist != null ? Results.Ok(playlist) : Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while adding the song to the playlist");
            }
        })
        .WithName("AddSongToPlaylist")
        .WithSummary("Add song to playlist")
        .WithDescription("Add a song to an existing playlist")
        .Produces<PlaylistDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        // DELETE /api/playlists/{id}/songs/{songId} - Remove song from playlist
        group.MapDelete("/{id:guid}/songs/{songId:guid}", async (Guid id, Guid songId, IPlaylistService playlistService) =>
        {
            try
            {
                var success = await playlistService.RemoveSongFromPlaylistAsync(id, songId);
                return success ? Results.NoContent() : Results.NotFound();
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while removing the song from the playlist");
            }
        })
        .WithName("RemoveSongFromPlaylist")
        .WithSummary("Remove song from playlist")
        .WithDescription("Remove a song from a playlist")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        // PUT /api/playlists/{id}/reorder - Reorder playlist songs
        group.MapPut("/{id:guid}/reorder", async (Guid id, [FromBody] ReorderPlaylistDto reorderDto, IPlaylistService playlistService) =>
        {
            try
            {
                var playlist = await playlistService.ReorderPlaylistAsync(id, reorderDto);
                return playlist != null ? Results.Ok(playlist) : Results.NotFound();
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while reordering the playlist");
            }
        })
        .WithName("ReorderPlaylist")
        .WithSummary("Reorder playlist songs")
        .WithDescription("Reorder the songs in a playlist")
        .Produces<PlaylistDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
