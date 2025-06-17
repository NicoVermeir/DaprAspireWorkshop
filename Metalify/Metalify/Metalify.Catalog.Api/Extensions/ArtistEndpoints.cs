using Metalify.Catalog.Api.Services;
using Metalify.Catalog.Api.DTOs;

namespace Metalify.Catalog.Api.Extensions;

public static class ArtistEndpoints
{
    public static void MapArtistEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/artists")
            .WithTags("Artists");

        group.MapGet("", async (IArtistService artistService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("{id:guid}", async (Guid id, IArtistService artistService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("search", async (string q, IArtistService artistService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("country/{country}", async (string country, IArtistService artistService) =>
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
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
