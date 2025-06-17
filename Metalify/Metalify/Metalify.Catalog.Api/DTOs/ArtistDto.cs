namespace Metalify.Catalog.Api.DTOs;

/// <summary>
/// Data transfer object for Artist information
/// </summary>
public class ArtistDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int? FormedYear { get; set; }
    public List<string> Genres { get; set; } = new();
    public List<AlbumSummaryDto> Albums { get; set; } = new();
}

/// <summary>
/// Summary DTO for Artist (without albums)
/// </summary>
public class ArtistSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int? FormedYear { get; set; }
    public List<string> Genres { get; set; } = new();
}
