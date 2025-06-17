using System.ComponentModel.DataAnnotations;

namespace Metalify.BandCenter.Models;

/// <summary>
/// Represents a metal band
/// </summary>
public class Band
{
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
    
    public int? FormedYear { get; set; }
    
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
    
    [MaxLength(500)]
    public string WebsiteUrl { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public List<Album> Albums { get; set; } = new();
}
