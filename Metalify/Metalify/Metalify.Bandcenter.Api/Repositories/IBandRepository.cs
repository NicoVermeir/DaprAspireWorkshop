using Metalify.Bandcenter.Api.Models;

namespace Metalify.Bandcenter.Api.Repositories;

public interface IBandRepository
{
    Task<IEnumerable<Band>> GetAllAsync();
    Task<Band?> GetByIdAsync(Guid id);
    Task<Band?> GetByIdWithAlbumsAsync(Guid id);
    Task<Band?> GetByNameAsync(string name);
    Task<IEnumerable<Band>> SearchByNameAsync(string searchTerm);
    Task<IEnumerable<Band>> GetByGenreAsync(string genre);
    Task<IEnumerable<Band>> GetByCountryAsync(string country);
    Task<Band> AddAsync(Band band);
    Task<Band> UpdateAsync(Band band);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
