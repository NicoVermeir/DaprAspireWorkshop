using Dapr;
using Metalify.Catalog.Api.DTOs;
using Metalify.Catalog.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Metalify.Catalog.Api.Extensions;

public static class SubscriptionEndpoints
{
    public static void MapSubscriptionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/subscriptions")
            .WithTags("Subscriptions");

        group.MapPost("/artistChanged", [Topic("pubsub", "artist-changed")] ([FromBody]ArtistDto artist, IArtistService artistService) => {
            Console.WriteLine("Artist change received : " + artist.Name);
            artistService.UpdateArtistAsync(artist);
            return Results.Ok(artist);
        });
    }
}