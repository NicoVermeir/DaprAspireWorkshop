using Metalify.Bandcenter.Api.Models;

namespace Metalify.Bandcenter.Api.Repositories;

public interface IBandRepository
{
    Task<IEnumerable<Artist>> GetAllAsync();
    Task<Artist?> GetByIdAsync(Guid id);
    Task<Artist?> GetByIdWithAlbumsAsync(Guid id);
    Task<Artist?> GetByNameAsync(string name);
    Task<IEnumerable<Artist>> SearchByNameAsync(string searchTerm);
    Task<IEnumerable<Artist>> GetByGenreAsync(string genre);
    Task<IEnumerable<Artist>> GetByCountryAsync(string country);
    Task<Artist> AddAsync(Artist band);
    Task<Artist> UpdateAsync(Artist band);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
