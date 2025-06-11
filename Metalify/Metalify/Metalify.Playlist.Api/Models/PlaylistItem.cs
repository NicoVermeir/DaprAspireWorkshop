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
    public Guid PlaylistId { get; set; }

    /// <summary>
    /// ID of the song in this playlist item
    /// </summary>
    [Required]
    public Guid SongId { get; set; }

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