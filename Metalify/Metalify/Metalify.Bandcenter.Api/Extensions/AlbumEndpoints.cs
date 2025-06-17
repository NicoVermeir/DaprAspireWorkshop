using Microsoft.AspNetCore.Mvc;
using Metalify.Bandcenter.Api.DTOs;
using Metalify.Bandcenter.Api.Services;

namespace Metalify.Bandcenter.Api.Extensions;

public static class AlbumEndpoints
{
    public static void MapAlbumEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/albums")
            .WithTags("Albums")
            .WithOpenApi();

        // GET /api/albums
        group.MapGet("/", async (IAlbumService albumService) =>
        {
            var albums = await albumService.GetAllAlbumsAsync();
            return Results.Ok(albums);
        })
        .WithName("GetAllAlbums")
        .WithSummary("Get all albums")
        .WithDescription("Retrieves a list of all albums in the catalog");        // GET /api/albums/{id}
        group.MapGet("/{id:guid}", async (Guid id, IAlbumService albumService) =>
        {
            var album = await albumService.GetAlbumByIdAsync(id);
            return album == null ? Results.NotFound($"Album with ID {id} not found") : Results.Ok(album);
        })
        .WithName("GetAlbumById")
        .WithSummary("Get album by ID")
        .WithDescription("Retrieves a specific album by its ID");

        // GET /api/albums/{id}/songs
        group.MapGet("/{id:guid}/songs", async (Guid id, IAlbumService albumService) =>
        {
            var album = await albumService.GetAlbumWithSongsAsync(id);
            return album == null ? Results.NotFound($"Album with ID {id} not found") : Results.Ok(album);
        })
        .WithName("GetAlbumWithSongs")
        .WithSummary("Get album with songs")
        .WithDescription("Retrieves an album with all its songs");

        // GET /api/albums/band/{bandId}
        group.MapGet("/band/{bandId:guid}", async (Guid bandId, IAlbumService albumService) =>
        {
            var albums = await albumService.GetAlbumsByBandIdAsync(bandId);
            return Results.Ok(albums);
        })
        .WithName("GetAlbumsByBand")
        .WithSummary("Get albums by band")
        .WithDescription("Retrieves all albums by a specific band");

        // GET /api/albums/search?title={searchTerm}
        group.MapGet("/search", async ([FromQuery] string title, IAlbumService albumService) =>
        {
            if (string.IsNullOrWhiteSpace(title))
                return Results.BadRequest("Search term 'title' is required");

            var albums = await albumService.SearchAlbumsByTitleAsync(title);
            return Results.Ok(albums);
        })
        .WithName("SearchAlbumsByTitle")
        .WithSummary("Search albums by title")
        .WithDescription("Searches for albums by title containing the specified term");        // GET /api/albums/genre/{genre}
        group.MapGet("/albumtype/{albumType}", async (string albumType, IAlbumService albumService) =>
        {
            var albums = await albumService.GetAlbumsByAlbumTypeAsync(albumType);
            return Results.Ok(albums);
        })
        .WithName("GetAlbumsByAlbumType")
        .WithSummary("Get albums by album type")
        .WithDescription("Retrieves albums that match the specified album type");

        // GET /api/albums/year/{year}
        group.MapGet("/year/{year:int}", async (int year, IAlbumService albumService) =>
        {
            var albums = await albumService.GetAlbumsByYearAsync(year);
            return Results.Ok(albums);
        })
        .WithName("GetAlbumsByYear")
        .WithSummary("Get albums by year")
        .WithDescription("Retrieves albums released in the specified year");

        // GET /api/albums/year-range?start={startYear}&end={endYear}
        group.MapGet("/year-range", async ([FromQuery] int start, [FromQuery] int end, IAlbumService albumService) =>
        {
            if (start <= 0 || end <= 0 || start > end)
                return Results.BadRequest("Valid start and end years are required (start <= end)");

            var albums = await albumService.GetAlbumsByYearRangeAsync(start, end);
            return Results.Ok(albums);
        })
        .WithName("GetAlbumsByYearRange")
        .WithSummary("Get albums by year range")
        .WithDescription("Retrieves albums released within the specified year range");

        // POST /api/albums
        group.MapPost("/", async ([FromBody] CreateAlbumDto createAlbumDto, IAlbumService albumService) =>
        {
            try
            {
                var album = await albumService.CreateAlbumAsync(createAlbumDto);
                return Results.CreatedAtRoute("GetAlbumById", new { id = album.Id }, album);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CreateAlbum")
        .WithSummary("Create a new album")
        .WithDescription("Creates a new album in the catalog");        // PUT /api/albums/{id}
        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateAlbumDto updateAlbumDto, IAlbumService albumService) =>
        {
            try
            {
                var album = await albumService.UpdateAlbumAsync(id, updateAlbumDto);
                return album == null ? Results.NotFound($"Album with ID {id} not found") : Results.Ok(album);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("UpdateAlbum")
        .WithSummary("Update an album")
        .WithDescription("Updates an existing album in the catalog");

        // DELETE /api/albums/{id}
        group.MapDelete("/{id:guid}", async (Guid id, IAlbumService albumService) =>
        {
            var deleted = await albumService.DeleteAlbumAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound($"Album with ID {id} not found");
        })
        .WithName("DeleteAlbum")
        .WithSummary("Delete an album")
        .WithDescription("Deletes an album from the catalog");
    }
}
