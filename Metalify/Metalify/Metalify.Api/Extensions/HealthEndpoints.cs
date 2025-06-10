namespace Metalify.Api.Extensions;

public static class HealthEndpoints
{
    public static void MapHealthEndpoints(this WebApplication app)
    {
        app.MapGet("/health", () => new { Status = "Healthy", Timestamp = DateTime.UtcNow })
            .WithName("HealthCheck")
            .WithSummary("Health check")
            .WithDescription("Returns the health status of the API")
            .Produces(StatusCodes.Status200OK)
            .WithTags("Health");
    }
}
