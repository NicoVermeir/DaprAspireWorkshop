using System.ComponentModel.DataAnnotations;

namespace Metalify.BandCenter.Models;

/// <summary>
/// Represents a metal album
/// </summary>
public class Album
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
      [Required]
    [MaxLength(50)]
    public string AlbumType { get; set; } = string.Empty;
    
    public int? ReleaseYear { get; set; }
    
    [MaxLength(500)]
    public string CoverImageUrl { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string Label { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string CatalogNumber { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Foreign key
    public Guid BandId { get; set; }
    
    // Navigation properties
    public Band? Band { get; set; }
    public List<Song> Songs { get; set; } = new();
}
