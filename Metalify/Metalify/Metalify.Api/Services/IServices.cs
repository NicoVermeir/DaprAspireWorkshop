using Metalify.Api.DTOs;

namespace Metalify.Api.Services;

/// <summary>
/// Service interface for Artist business logic
/// </summary>
public interface IArtistService
{
    Task<IEnumerable<ArtistSummaryDto>> GetAllArtistsAsync();
    Task<ArtistDto?> GetArtistByIdAsync(Guid id);
    Task<IEnumerable<ArtistSummaryDto>> GetArtistsByCountryAsync(string country);
    Task<IEnumerable<ArtistSummaryDto>> SearchArtistsAsync(string searchTerm);
}

/// <summary>
/// Service interface for Album business logic
/// </summary>
public interface IAlbumService
{
    Task<IEnumerable<AlbumSummaryDto>> GetAllAlbumsAsync();
    Task<AlbumDto?> GetAlbumByIdAsync(Guid id);
    Task<IEnumerable<AlbumSummaryDto>> GetAlbumsByArtistIdAsync(Guid artistId);
    Task<IEnumerable<AlbumSummaryDto>> GetAlbumsByYearAsync(int year);
    Task<IEnumerable<AlbumSummaryDto>> SearchAlbumsAsync(string searchTerm);
}

/// <summary>
/// Service interface for Song business logic
/// </summary>
public interface ISongService
{
    Task<IEnumerable<SongDto>> GetAllSongsAsync();
    Task<SongDto?> GetSongByIdAsync(Guid id);
    Task<IEnumerable<SongDto>> GetSongsByAlbumIdAsync(Guid albumId);
    Task<IEnumerable<SongDto>> GetSongsByArtistIdAsync(Guid artistId);
    Task<IEnumerable<SongDto>> SearchSongsAsync(string searchTerm);
}
