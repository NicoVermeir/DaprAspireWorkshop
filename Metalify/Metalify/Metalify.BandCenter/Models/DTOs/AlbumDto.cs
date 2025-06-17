namespace Metalify.BandCenter.Models.DTOs;

/// <summary>
/// Data transfer object for Album information
/// </summary>
public class AlbumDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AlbumType { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string CoverImageUrl { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string CatalogNumber { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public TimeSpan TotalDuration { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Guid BandId { get; set; }
    public string BandName { get; set; } = string.Empty;
    public List<SongDto> Songs { get; set; } = new();
}

/// <summary>
/// Summary DTO for Album information without songs
/// </summary>
public class AlbumSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AlbumType { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string CoverImageUrl { get; set; } = string.Empty;
    public string BandName { get; set; } = string.Empty;
    public TimeSpan TotalDuration { get; set; }
    public int SongCount { get; set; }
}

/// <summary>
/// DTO for creating a new album
/// </summary>
public class CreateAlbumDto
{
    public string Title { get; set; } = string.Empty;
    public string AlbumType { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string CoverImageUrl { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string CatalogNumber { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public Guid BandId { get; set; }
}

/// <summary>
/// DTO for updating album information
/// </summary>
public class UpdateAlbumDto
{
    public string? Title { get; set; }
    public string? AlbumType { get; set; }
    public int? ReleaseYear { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Label { get; set; }
    public string? CatalogNumber { get; set; }
    public string? Format { get; set; }
    public string? Notes { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Alias for CreateAlbumDto to match common naming convention
/// </summary>
public class AlbumCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? AlbumType { get; set; }
    public int? ReleaseYear { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Label { get; set; }
    public string? CatalogNumber { get; set; }
    public string? Format { get; set; }
    public string? Notes { get; set; }
    public string? Description { get; set; }
    public Guid BandId { get; set; }
}
