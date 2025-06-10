using Metalify.Api.Models;

namespace Metalify.Api.Repositories;

/// <summary>
/// Repository interface for Artist operations
/// </summary>
public interface IArtistRepository
{
    Task<IEnumerable<Artist>> GetAllAsync();
    Task<Artist?> GetByIdAsync(Guid id);
    Task<IEnumerable<Artist>> GetByCountryAsync(string country);
    Task<IEnumerable<Artist>> SearchByNameAsync(string searchTerm);
}

/// <summary>
/// Repository interface for Album operations
/// </summary>
public interface IAlbumRepository
{
    Task<IEnumerable<Album>> GetAllAsync();
    Task<Album?> GetByIdAsync(Guid id);
    Task<IEnumerable<Album>> GetByArtistIdAsync(Guid artistId);
    Task<IEnumerable<Album>> GetByYearAsync(int year);
    Task<IEnumerable<Album>> SearchByTitleAsync(string searchTerm);
}

/// <summary>
/// Repository interface for Song operations
/// </summary>
public interface ISongRepository
{
    Task<IEnumerable<Song>> GetAllAsync();
    Task<Song?> GetByIdAsync(Guid id);
    Task<IEnumerable<Song>> GetByAlbumIdAsync(Guid albumId);
    Task<IEnumerable<Song>> GetByArtistIdAsync(Guid artistId);
    Task<IEnumerable<Song>> SearchByTitleAsync(string searchTerm);
}
