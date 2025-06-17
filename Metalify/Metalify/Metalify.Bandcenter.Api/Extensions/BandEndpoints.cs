using Microsoft.AspNetCore.Mvc;
using Metalify.Bandcenter.Api.DTOs;
using Metalify.Bandcenter.Api.Services;

namespace Metalify.Bandcenter.Api.Extensions;

public static class BandEndpoints
{
    public static void MapBandEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/bands")
            .WithTags("Bands")
            .WithOpenApi();

        // GET /api/bands
        group.MapGet("/", async (IBandService bandService) =>
        {
            var bands = await bandService.GetAllBandsAsync();
            return Results.Ok(bands);
        })
        .WithName("GetAllBands")
        .WithSummary("Get all bands")
        .WithDescription("Retrieves a list of all bands in the catalog");        // GET /api/bands/{id}
        group.MapGet("/{id:guid}", async (Guid id, IBandService bandService) =>
        {
            var band = await bandService.GetBandByIdAsync(id);
            return band == null ? Results.NotFound($"Band with ID {id} not found") : Results.Ok(band);
        })
        .WithName("GetBandById")
        .WithSummary("Get band by ID")
        .WithDescription("Retrieves a specific band by its ID");

        // GET /api/bands/{id}/albums
        group.MapGet("/{id:guid}/albums", async (Guid id, IBandService bandService) =>
        {
            var band = await bandService.GetBandWithAlbumsAsync(id);
            return band == null ? Results.NotFound($"Band with ID {id} not found") : Results.Ok(band);
        })
        .WithName("GetBandWithAlbums")
        .WithSummary("Get band with albums")
        .WithDescription("Retrieves a band with all its albums and songs");

        // GET /api/bands/search?name={searchTerm}
        group.MapGet("/search", async ([FromQuery] string name, IBandService bandService) =>
        {
            if (string.IsNullOrWhiteSpace(name))
                return Results.BadRequest("Search term 'name' is required");

            var bands = await bandService.SearchBandsByNameAsync(name);
            return Results.Ok(bands);
        })
        .WithName("SearchBandsByName")
        .WithSummary("Search bands by name")
        .WithDescription("Searches for bands by name containing the specified term");

        // GET /api/bands/genre/{genre}
        group.MapGet("/genre/{genre}", async (string genre, IBandService bandService) =>
        {
            var bands = await bandService.GetBandsByGenreAsync(genre);
            return Results.Ok(bands);
        })
        .WithName("GetBandsByGenre")
        .WithSummary("Get bands by genre")
        .WithDescription("Retrieves bands that match the specified genre");

        // GET /api/bands/country/{country}
        group.MapGet("/country/{country}", async (string country, IBandService bandService) =>
        {
            var bands = await bandService.GetBandsByCountryAsync(country);
            return Results.Ok(bands);
        })
        .WithName("GetBandsByCountry")
        .WithSummary("Get bands by country")
        .WithDescription("Retrieves bands from the specified country");

        // POST /api/bands
        group.MapPost("/", async ([FromBody] CreateBandDto createBandDto, IBandService bandService) =>
        {
            try
            {
                var band = await bandService.CreateBandAsync(createBandDto);
                return Results.CreatedAtRoute("GetBandById", new { id = band.Id }, band);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CreateBand")
        .WithSummary("Create a new band")
        .WithDescription("Creates a new band in the catalog");        // PUT /api/bands/{id}
        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateBandDto updateBandDto, IBandService bandService) =>
        {
            try
            {
                var band = await bandService.UpdateBandAsync(id, updateBandDto);
                return band == null ? Results.NotFound($"Band with ID {id} not found") : Results.Ok(band);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("UpdateBand")
        .WithSummary("Update a band")
        .WithDescription("Updates an existing band in the catalog");

        // DELETE /api/bands/{id}
        group.MapDelete("/{id:guid}", async (Guid id, IBandService bandService) =>
        {
            var deleted = await bandService.DeleteBandAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound($"Band with ID {id} not found");
        })
        .WithName("DeleteBand")
        .WithSummary("Delete a band")
        .WithDescription("Deletes a band from the catalog");
    }
}
