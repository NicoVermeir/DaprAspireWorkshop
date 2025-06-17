using Metalify.Bandcenter.Api.Models;

namespace Metalify.Bandcenter.Api.Repositories;

public interface ISongRepository
{
    Task<IEnumerable<Song>> GetAllAsync();
    Task<Song?> GetByIdAsync(Guid id);
    Task<IEnumerable<Song>> GetByAlbumIdAsync(Guid albumId);
    Task<IEnumerable<Song>> GetByBandIdAsync(Guid bandId);
    Task<IEnumerable<Song>> SearchByTitleAsync(string searchTerm);
    Task<IEnumerable<Song>> GetByDurationRangeAsync(TimeSpan minDuration, TimeSpan maxDuration);
    Task<Song> AddAsync(Song song);
    Task<Song> UpdateAsync(Song song);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
