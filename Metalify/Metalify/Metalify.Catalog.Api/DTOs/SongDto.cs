namespace Metalify.Catalog.Api.DTOs;

/// <summary>
/// Data transfer object for Song information
/// </summary>
public class SongDto
{
    /// <summary>
    /// Unique identifier for the song
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Title of the song
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Track number on the album
    /// </summary>
    public int TrackNumber { get; set; }
    
    /// <summary>
    /// Duration of the song
    /// </summary>
    public TimeSpan Duration { get; set; }
    
    /// <summary>
    /// ID of the album this song belongs to
    /// </summary>
    public Guid AlbumId { get; set; }
    
    /// <summary>
    /// Title of the album this song belongs to
    /// </summary>
    public string AlbumTitle { get; set; } = string.Empty;
    
    /// <summary>
    /// Cover image URL of the album this song belongs to
    /// </summary>
    public string AlbumCoverImageUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// ID of the artist who performed this song
    /// </summary>
    public Guid ArtistId { get; set; }
    
    /// <summary>
    /// Name of the artist who performed this song
    /// </summary>
    public string ArtistName { get; set; } = string.Empty;
    
    /// <summary>
    /// URL to the audio file for this song
    /// </summary>
    public string AudioUrl { get; set; } = string.Empty;
}
