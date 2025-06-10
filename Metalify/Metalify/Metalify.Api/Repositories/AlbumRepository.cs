using Microsoft.EntityFrameworkCore;
using Metalify.Api.Data;
using Metalify.Api.Models;

namespace Metalify.Api.Repositories;

/// <summary>
/// Entity Framework implementation of the Album repository
/// </summary>
public class AlbumRepository : IAlbumRepository
{
    private readonly MetalifyCatalogDbContext _context;
    private readonly ILogger<AlbumRepository> _logger;

    public AlbumRepository(MetalifyCatalogDbContext context, ILogger<AlbumRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Album>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all albums");
            return await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .OrderBy(a => a.Title)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all albums");
            throw;
        }
    }

    public async Task<Album?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Retrieving album with ID: {AlbumId}", id);
            return await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs.OrderBy(s => s.TrackNumber))
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving album with ID: {AlbumId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Album>> GetByArtistIdAsync(Guid artistId)
    {
        try
        {
            _logger.LogInformation("Retrieving albums for artist ID: {ArtistId}", artistId);
            return await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .Where(a => a.ArtistId == artistId)
                .OrderBy(a => a.ReleaseYear)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving albums for artist ID: {ArtistId}", artistId);
            throw;
        }
    }

    public async Task<IEnumerable<Album>> GetByYearAsync(int year)
    {
        try
        {
            _logger.LogInformation("Retrieving albums from year: {Year}", year);
            return await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .Where(a => a.ReleaseYear == year)
                .OrderBy(a => a.Title)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving albums from year: {Year}", year);
            throw;
        }
    }

    public async Task<IEnumerable<Album>> SearchByTitleAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation("Searching albums with term: {SearchTerm}", searchTerm);
            return await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .Where(a => a.Title.ToLower().Contains(searchTerm.ToLower()))
                .OrderBy(a => a.Title)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching albums with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
