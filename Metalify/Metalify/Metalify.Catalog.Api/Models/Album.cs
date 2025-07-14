using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metalify.Catalog.Api.Models;

/// <summary>
/// Represents a metal album entity
/// </summary>
public class Album
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string AlbumType { get; set; } = string.Empty;

    public int ReleaseYear { get; set; }

    [MaxLength(500)]
    public string CoverImageUrl { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Label { get; set; } = string.Empty;

    [MaxLength(100)]
    public string CatalogNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Format { get; set; } = string.Empty;

    public TimeSpan TotalDuration { get; set; }

    public string Notes { get; set; } = string.Empty;

    [Required]
    public Guid ArtistId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(ArtistId))]
    public virtual Artist Artist { get; set; } = null!;
    
    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

    // Legacy properties for backward compatibility
    public int? ReleaseYear_Legacy
    {
        get => ReleaseYear == 0 ? null : ReleaseYear;
        set => ReleaseYear = value ?? 0;
    }
}
