namespace Metalify.Api.DTOs;

/// <summary>
/// Data transfer object for Song information
/// </summary>
public class SongDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int TrackNumber { get; set; }
    public TimeSpan Duration { get; set; }
    public Guid AlbumId { get; set; }
    public string AlbumTitle { get; set; } = string.Empty;
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
}
