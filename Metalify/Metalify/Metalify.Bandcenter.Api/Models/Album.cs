using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metalify.Bandcenter.Api.Models;

/// <summary>
/// Represents a metal album entity
/// </summary>
public class Album
{
    /// <summary>
    /// Unique identifier for the album
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Title of the album
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Album type (Full-length, EP, Demo, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string AlbumType { get; set; } = string.Empty;

    /// <summary>
    /// Year the album was released
    /// </summary>
    public int ReleaseYear { get; set; }

    /// <summary>
    /// Album cover art image URL
    /// </summary>
    [MaxLength(500)]
    public string CoverImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Record label that released the album
    /// </summary>
    [MaxLength(200)]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Catalog number or identifier from the label
    /// </summary>
    [MaxLength(100)]
    public string CatalogNumber { get; set; } = string.Empty;

    /// <summary>
    /// Format of the release (CD, Vinyl, Digital, etc.)
    /// </summary>
    [MaxLength(100)]
    public string Format { get; set; } = string.Empty;

    /// <summary>
    /// Total duration of the album
    /// </summary>
    public TimeSpan TotalDuration { get; set; }

    /// <summary>
    /// Additional notes about the album
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// ID of the band that created this album
    /// </summary>
    [Required]
    public Guid BandId { get; set; }

    /// <summary>
    /// When the album record was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the album record was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    /// <summary>
    /// The band that created this album
    /// </summary>
    [ForeignKey(nameof(BandId))]
    public virtual Band Band { get; set; } = null!;

    /// <summary>
    /// Songs on this album
    /// </summary>
    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
