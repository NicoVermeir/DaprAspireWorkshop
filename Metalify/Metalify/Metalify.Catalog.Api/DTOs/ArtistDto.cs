namespace Metalify.Catalog.Api.DTOs;

/// <summary>
/// Data transfer object for Artist information
/// </summary>
public class ArtistDto
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

    // Legacy properties for backward compatibility
    public string ImageUrl 
    { 
        get => PhotoUrl; 
        set => PhotoUrl = value; 
    }
    
    public string Bio 
    { 
        get => Biography; 
        set => Biography = value; 
    }
    
    public List<string> Genres 
    { 
        get => string.IsNullOrEmpty(Genre) ? new() : Genre.Split(',').ToList(); 
        set => Genre = string.Join(",", value); 
    }
}

/// <summary>
/// Summary DTO for Artist (without albums)
/// </summary>
public class ArtistSummaryDto
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

    // Legacy properties for backward compatibility
    public string ImageUrl 
    { 
        get => PhotoUrl; 
        set => PhotoUrl = value; 
    }
    
    public List<string> Genres 
    { 
        get => string.IsNullOrEmpty(Genre) ? new() : Genre.Split(',').ToList(); 
        set => Genre = string.Join(",", value); 
    }
}

/// <summary>
/// DTO for creating a new artist
/// </summary>
public class CreateArtistDto
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
/// DTO for updating an existing artist
/// </summary>
public class UpdateArtistDto
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
