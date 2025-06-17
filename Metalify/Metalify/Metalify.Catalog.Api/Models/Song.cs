using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metalify.Catalog.Api.Models;

/// <summary>
/// Represents a metal song entity
/// </summary>
public class Song
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public int TrackNumber { get; set; }

    public TimeSpan Duration { get; set; }

    [Required]
    public Guid AlbumId { get; set; }

    [Required]
    public Guid ArtistId { get; set; }

    [MaxLength(500)]
    public string AudioUrl { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(AlbumId))]
    public virtual Album Album { get; set; } = null!;

    [ForeignKey(nameof(ArtistId))]
    public virtual Artist Artist { get; set; } = null!;
}
