using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metalify.Api.Models;

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

    [MaxLength(500)]
    public string CoverImageUrl { get; set; } = string.Empty;

    public int? ReleaseYear { get; set; }

    [Required]
    public Guid ArtistId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(ArtistId))]
    public virtual Artist Artist { get; set; } = null!;
    
    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
