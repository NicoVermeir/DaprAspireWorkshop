namespace Metalify.BandCenter.Models.DTOs;

/// <summary>
/// Data transfer object for Song information
/// </summary>
public class SongDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int TrackNumber { get; set; }
    public TimeSpan Duration { get; set; }
    public string Lyrics { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public Guid AlbumId { get; set; }
    public string AlbumTitle { get; set; } = string.Empty;
    public Guid BandId { get; set; }
    public string BandName { get; set; } = string.Empty;
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
    public string BandName { get; set; } = string.Empty;
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
    public Guid BandId { get; set; }
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
}
