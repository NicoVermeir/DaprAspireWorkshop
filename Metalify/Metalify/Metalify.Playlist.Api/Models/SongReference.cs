namespace Metalify.Playlist.Api.Models;

/// <summary>
/// Simplified song model for playlist references
/// This represents songs from the main catalog API
/// </summary>
public class SongReference
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public string AlbumTitle { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
}