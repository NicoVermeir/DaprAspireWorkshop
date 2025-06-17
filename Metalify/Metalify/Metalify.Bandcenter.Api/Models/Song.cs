using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metalify.Bandcenter.Api.Models;

/// <summary>
/// Represents a metal song entity
/// </summary>
public class Song
{
    /// <summary>
    /// Unique identifier for the song
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Title of the song
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Track number on the album
    /// </summary>
    public int TrackNumber { get; set; }

    /// <summary>
    /// Duration of the song
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Lyrics of the song
    /// </summary>
    public string Lyrics { get; set; } = string.Empty;

    /// <summary>
    /// Additional notes about the song
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// ID of the album this song belongs to
    /// </summary>
    [Required]
    public Guid AlbumId { get; set; }

    /// <summary>
    /// ID of the band that performed this song
    /// </summary>
    [Required]
    public Guid BandId { get; set; }

    /// <summary>
    /// When the song record was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the song record was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    /// <summary>
    /// The album this song belongs to
    /// </summary>
    [ForeignKey(nameof(AlbumId))]
    public virtual Album Album { get; set; } = null!;

    /// <summary>
    /// The band that performed this song
    /// </summary>
    [ForeignKey(nameof(BandId))]
    public virtual Band Band { get; set; } = null!;
}
