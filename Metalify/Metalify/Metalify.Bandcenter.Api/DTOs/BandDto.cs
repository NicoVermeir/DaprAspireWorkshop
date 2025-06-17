namespace Metalify.Bandcenter.Api.DTOs;

/// <summary>
/// Data transfer object for Band information
/// </summary>
public class BandDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int FormedYear { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string Themes { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string YearsActive { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<AlbumSummaryDto> Albums { get; set; } = new();
}

/// <summary>
/// Summary DTO for Band information without albums
/// </summary>
public class BandSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int FormedYear { get; set; }
    public string LogoUrl { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public int AlbumCount { get; set; }
}

/// <summary>
/// DTO for creating a new band
/// </summary>
public class CreateBandDto
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int FormedYear { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string Themes { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string YearsActive { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
}

/// <summary>
/// DTO for updating an existing band
/// </summary>
public class UpdateBandDto
{
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    public int? FormedYear { get; set; }
    public string? Genre { get; set; }
    public string? Themes { get; set; }
    public string? Label { get; set; }
    public string? YearsActive { get; set; }
    public string? LogoUrl { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Biography { get; set; }
}
