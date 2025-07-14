namespace Metalify.Catalog.Api.DTOs;

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
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<SongDto> Songs { get; set; } = new();

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

    // Legacy properties for backward compatibility
    public int? ReleaseYear_Legacy
    {
        get => ReleaseYear == 0 ? null : ReleaseYear;
        set => ReleaseYear = value ?? 0;
    }
}

/// <summary>
/// Summary DTO for Album (without songs)
/// </summary>
public class AlbumSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AlbumType { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string CoverImageUrl { get; set; } = string.Empty;
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public TimeSpan TotalDuration { get; set; }
    public int SongCount { get; set; }

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

    // Legacy properties for backward compatibility
    public int? ReleaseYear_Legacy
    {
        get => ReleaseYear == 0 ? null : ReleaseYear;
        set => ReleaseYear = value ?? 0;
    }
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
    public Guid ArtistId { get; set; }
}

/// <summary>
/// DTO for updating an existing album
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
}
