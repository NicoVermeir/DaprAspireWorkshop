namespace Metalify.Playlist.Api.DTOs;

/// <summary>
/// Full playlist DTO with all details including songs
/// </summary>
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

/// <summary>
/// Summary playlist DTO for lists and search results
/// </summary>
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

/// <summary>
/// DTO for creating a new playlist
/// </summary>
public class CreatePlaylistDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
}

/// <summary>
/// DTO for updating an existing playlist
/// </summary>
public class UpdatePlaylistDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool? IsPublic { get; set; }
}

/// <summary>
/// DTO representing a song in a playlist
/// </summary>
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

/// <summary>
/// DTO for adding a song to a playlist
/// </summary>
public class AddSongToPlaylistDto
{
    public Guid SongId { get; set; }
    public string SongTitle { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public string AlbumTitle { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int? Position { get; set; } // If null, add to end
}

/// <summary>
/// DTO for reordering songs in a playlist
/// </summary>
public class ReorderPlaylistDto
{
    public List<PlaylistItemOrderDto> Items { get; set; } = new();
}

/// <summary>
/// DTO for playlist item order
/// </summary>
public class PlaylistItemOrderDto
{
    public Guid ItemId { get; set; }
    public int Position { get; set; }
}

/// <summary>
/// DTO for song data from the catalog API
/// Used when calling the main catalog service for song details
/// </summary>
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
