using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metalify.Catalog.Api.Models;

/// <summary>
/// Represents a metal artist/band entity
/// </summary>
public class Artist
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    public int FormedYear { get; set; }

    [Required]
    [MaxLength(100)]
    public string Genre { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Themes { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Label { get; set; } = string.Empty;

    [MaxLength(100)]
    public string YearsActive { get; set; } = string.Empty;

    [MaxLength(500)]
    public string LogoUrl { get; set; } = string.Empty;

    [MaxLength(500)]
    public string PhotoUrl { get; set; } = string.Empty;

    public string Biography { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    // Legacy properties for backward compatibility
    [MaxLength(500)]
    public string ImageUrl
    {
        get => PhotoUrl;
        set => PhotoUrl = value;
    }

    [MaxLength(2000)]
    public string Bio
    {
        get => Biography;
        set => Biography = value;
    }

    [MaxLength(500)]
    public string Genres
    {
        get => Genre;
        set => Genre = value;
    }

    public int? FormedYear_Legacy
    {
        get => FormedYear == 0 ? null : FormedYear;
        set => FormedYear = value ?? 0;
    }
}
