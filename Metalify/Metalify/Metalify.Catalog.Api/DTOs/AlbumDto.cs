namespace Metalify.Catalog.Api.DTOs;

/// <summary>
/// Data transfer object for Album information
/// </summary>
public class AlbumDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public int? ReleaseYear { get; set; }
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public List<SongDto> Songs { get; set; } = new();
}

/// <summary>
/// Summary DTO for Album (without songs)
/// </summary>
public class AlbumSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public int? ReleaseYear { get; set; }
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public int SongCount { get; set; }
}
