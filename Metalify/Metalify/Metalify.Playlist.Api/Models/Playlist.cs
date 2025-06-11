using System.ComponentModel.DataAnnotations;

namespace Metalify.Playlist.Api.Models;

/// <summary>
/// Represents a playlist entity in the Metalify catalog
/// </summary>
public class Playlist
{
    /// <summary>
    /// Unique identifier for the playlist
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the playlist
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the playlist
    /// </summary>
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Cover image URL for the playlist
    /// </summary>
    [StringLength(500)]
    public string CoverImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Whether the playlist is public or private
    /// </summary>
    public bool IsPublic { get; set; } = true;

    /// <summary>
    /// User ID who created the playlist
    /// </summary>
    [Required]
    [StringLength(100)]
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// When the playlist was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the playlist was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    /// <summary>
    /// Collection of playlist items (songs in this playlist)
    /// </summary>
    public virtual ICollection<PlaylistItem> PlaylistItems { get; set; } = new List<PlaylistItem>();
}