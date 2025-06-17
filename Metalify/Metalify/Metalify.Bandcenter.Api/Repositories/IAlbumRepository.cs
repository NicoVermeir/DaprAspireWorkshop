using Metalify.Bandcenter.Api.Models;

namespace Metalify.Bandcenter.Api.Repositories;

public interface IAlbumRepository
{
    Task<IEnumerable<Album>> GetAllAsync();
    Task<Album?> GetByIdAsync(Guid id);
    Task<Album?> GetByIdWithSongsAsync(Guid id);
    Task<IEnumerable<Album>> GetByBandIdAsync(Guid bandId);
    Task<IEnumerable<Album>> SearchByTitleAsync(string searchTerm);
    Task<IEnumerable<Album>> GetByAlbumTypeAsync(string albumType);
    Task<IEnumerable<Album>> GetByYearAsync(int year);
    Task<IEnumerable<Album>> GetByYearRangeAsync(int startYear, int endYear);
    Task<Album> AddAsync(Album album);
    Task<Album> UpdateAsync(Album album);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
