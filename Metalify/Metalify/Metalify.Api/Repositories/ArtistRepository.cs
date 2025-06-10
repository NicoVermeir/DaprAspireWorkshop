using Microsoft.EntityFrameworkCore;
using Metalify.Api.Data;
using Metalify.Api.Models;

namespace Metalify.Api.Repositories;

/// <summary>
/// Entity Framework implementation of the Artist repository
/// </summary>
public class ArtistRepository : IArtistRepository
{
    private readonly MetalifyCatalogDbContext _context;
    private readonly ILogger<ArtistRepository> _logger;

    public ArtistRepository(MetalifyCatalogDbContext context, ILogger<ArtistRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Artist>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all artists");
            return await _context.Artists
                .Include(a => a.Albums)
                .OrderBy(a => a.Name)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all artists");
            throw;
        }
    }

    public async Task<Artist?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Retrieving artist with ID: {ArtistId}", id);
            return await _context.Artists
                .Include(a => a.Albums)
                    .ThenInclude(album => album.Songs)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving artist with ID: {ArtistId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Artist>> GetByCountryAsync(string country)
    {
        try
        {
            _logger.LogInformation("Retrieving artists from country: {Country}", country);
            return await _context.Artists
                .Include(a => a.Albums)
                .Where(a => a.Country.ToLower().Contains(country.ToLower()))
                .OrderBy(a => a.Name)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving artists from country: {Country}", country);
            throw;
        }
    }

    public async Task<IEnumerable<Artist>> SearchByNameAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation("Searching artists with term: {SearchTerm}", searchTerm);
            return await _context.Artists
                .Include(a => a.Albums)
                .Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()))
                .OrderBy(a => a.Name)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching artists with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
