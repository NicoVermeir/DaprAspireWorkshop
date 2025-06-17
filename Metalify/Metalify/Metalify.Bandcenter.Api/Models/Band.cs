using System.ComponentModel.DataAnnotations;

namespace Metalify.Bandcenter.Api.Models;

/// <summary>
/// Represents a metal band entity
/// </summary>
public class Band
{
    /// <summary>
    /// Unique identifier for the band
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the band
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Country of origin
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Current location of the band
    /// </summary>
    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the band (Active, Split up, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Year the band was formed
    /// </summary>
    public int FormedYear { get; set; }

    /// <summary>
    /// Primary metal genre
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Genre { get; set; } = string.Empty;

    /// <summary>
    /// Lyrical themes
    /// </summary>
    [MaxLength(500)]
    public string Themes { get; set; } = string.Empty;

    /// <summary>
    /// Current record label
    /// </summary>
    [MaxLength(200)]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Years the band was active
    /// </summary>
    [MaxLength(100)]
    public string YearsActive { get; set; } = string.Empty;

    /// <summary>
    /// Band logo image URL
    /// </summary>
    [MaxLength(500)]
    public string LogoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Band photo image URL
    /// </summary>
    [MaxLength(500)]
    public string PhotoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Band biography or description
    /// </summary>
    public string Biography { get; set; } = string.Empty;

    /// <summary>
    /// When the band record was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the band record was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    /// <summary>
    /// Albums released by this band
    /// </summary>
    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
}
