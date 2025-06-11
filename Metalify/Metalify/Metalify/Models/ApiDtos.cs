// API DTOs - Match the API response structure
namespace Metalify.Client.Models;

// API DTOs for communication with the Metalify.Api
public class ArtistDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int FormedYear { get; set; }
    public List<string> Genres { get; set; } = new();
    public List<AlbumSummaryDto>? Albums { get; set; }
}

public class ArtistSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int FormedYear { get; set; }
    public List<string> Genres { get; set; } = new();
}

public class AlbumDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public List<SongDto>? Songs { get; set; }
}

public class AlbumSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public int SongCount { get; set; }
}

public class SongDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int TrackNumber { get; set; }
    public TimeSpan Duration { get; set; }
    public Guid AlbumId { get; set; }
    public string AlbumTitle { get; set; } = string.Empty;
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
}

// Playlist API DTOs for communication with the Metalify.Playlist.Api
public class PlaylistDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int SongCount { get; set; }
    public TimeSpan TotalDuration { get; set; }
    public List<PlaylistItemDto>? Songs { get; set; }
}

public class PlaylistSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int SongCount { get; set; }
    public TimeSpan TotalDuration { get; set; }
}

public class CreatePlaylistDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
}

public class PlaylistItemDto
{
    public Guid Id { get; set; }
    public Guid SongId { get; set; }
    public string SongTitle { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public string AlbumTitle { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int Position { get; set; }
    public DateTime AddedAt { get; set; }
}

public class AddSongToPlaylistDto
{
    public Guid SongId { get; set; }
    public int? Position { get; set; } // If null, add to end
}

public class UpdatePlaylistDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
}
