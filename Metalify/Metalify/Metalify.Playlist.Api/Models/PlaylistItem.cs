using System.ComponentModel.DataAnnotations;

namespace Metalify.Playlist.Api.Models;

/// <summary>
/// Represents a song item within a playlist
/// </summary>
public class PlaylistItem
{
    /// <summary>
    /// Unique identifier for the playlist item
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// ID of the playlist this item belongs to
    /// </summary>
    [Required]
    public Guid PlaylistId { get; set; }    /// <summary>
    /// ID of the song in this playlist item
    /// </summary>
    [Required]
    public Guid SongId { get; set; }

    /// <summary>
    /// Denormalized song title for better performance and independence
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string SongTitle { get; set; } = string.Empty;

    /// <summary>
    /// Denormalized artist name for better performance and independence
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string ArtistName { get; set; } = string.Empty;    /// <summary>
    /// Denormalized album title for better performance and independence
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string AlbumTitle { get; set; } = string.Empty;

    /// <summary>
    /// Denormalized album cover image URL for better performance and independence
    /// </summary>
    [MaxLength(500)]
    public string AlbumCoverImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Denormalized song duration for better performance and independence
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Order of the song in the playlist (1-based)
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// When this item was added to the playlist
    /// </summary>
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    /// <summary>
    /// The playlist this item belongs to
    /// </summary>
    public virtual Playlist Playlist { get; set; } = null!;
}