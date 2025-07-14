namespace Metalify.Catalog.Api.DTOs;

/// <summary>
/// Data transfer object for Song information
/// </summary>
public class SongDto
{
    /// <summary>
    /// Unique identifier for the song
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Title of the song
    /// </summary>
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
    public Guid AlbumId { get; set; }
    
    /// <summary>
    /// Title of the album this song belongs to
    /// </summary>
    public string AlbumTitle { get; set; } = string.Empty;
    
    /// <summary>
    /// Cover image URL of the album this song belongs to
    /// </summary>
    public string AlbumCoverImageUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// ID of the artist who performed this song
    /// </summary>
    public Guid ArtistId { get; set; }
    
    /// <summary>
    /// Name of the artist who performed this song
    /// </summary>
    public string ArtistName { get; set; } = string.Empty;
    
    /// <summary>
    /// URL to the audio file for this song
    /// </summary>
    public string AudioUrl { get; set; } = string.Empty;

    /// <summary>
    /// When the song record was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the song record was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    // Compatibility properties for Band terminology
    public Guid BandId 
    { 
        get => ArtistId; 
        set => ArtistId = value; 
    }
    
    public string BandName 
    { 
        get => ArtistName; 
        set => ArtistName = value; 
    }
}

/// <summary>
/// Summary DTO for Song information
/// </summary>
public class SongSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int TrackNumber { get; set; }
    public TimeSpan Duration { get; set; }
    public string AlbumTitle { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;

    // Compatibility properties for Band terminology
    public string BandName 
    { 
        get => ArtistName; 
        set => ArtistName = value; 
    }
}

/// <summary>
/// DTO for creating a new song
/// </summary>
public class CreateSongDto
{
    public string Title { get; set; } = string.Empty;
    public int TrackNumber { get; set; }
    public TimeSpan Duration { get; set; }
    public string Lyrics { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public Guid AlbumId { get; set; }
    public Guid ArtistId { get; set; }
    public string AudioUrl { get; set; } = string.Empty;
}

/// <summary>
/// DTO for updating an existing song
/// </summary>
public class UpdateSongDto
{
    public string? Title { get; set; }
    public int? TrackNumber { get; set; }
    public TimeSpan? Duration { get; set; }
    public string? Lyrics { get; set; }
    public string? Notes { get; set; }
    public string? AudioUrl { get; set; }
}
