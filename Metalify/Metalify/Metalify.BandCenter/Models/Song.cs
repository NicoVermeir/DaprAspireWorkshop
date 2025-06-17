using System.ComponentModel.DataAnnotations;

namespace Metalify.BandCenter.Models;

/// <summary>
/// Represents a metal song
/// </summary>
public class Song
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public int TrackNumber { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public string Lyrics { get; set; } = string.Empty;
    
    public string Notes { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Foreign key
    [Required]
    public Guid AlbumId { get; set; }
    
    // Navigation property
    public Album? Album { get; set; }
}
