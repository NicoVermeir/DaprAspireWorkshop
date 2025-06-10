using Microsoft.EntityFrameworkCore;
using Metalify.Api.Data;
using Metalify.Api.Models;

namespace Metalify.Api.Repositories;

/// <summary>
/// Entity Framework implementation of the Song repository
/// </summary>
public class SongRepository : ISongRepository
{
    private readonly MetalifyCatalogDbContext _context;
    private readonly ILogger<SongRepository> _logger;

    public SongRepository(MetalifyCatalogDbContext context, ILogger<SongRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Song>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all songs");
            return await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .OrderBy(s => s.Artist.Name)
                .ThenBy(s => s.Album.Title)
                .ThenBy(s => s.TrackNumber)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all songs");
            throw;
        }
    }

    public async Task<Song?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Retrieving song with ID: {SongId}", id);
            return await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving song with ID: {SongId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Song>> GetByAlbumIdAsync(Guid albumId)
    {
        try
        {
            _logger.LogInformation("Retrieving songs for album ID: {AlbumId}", albumId);
            return await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .Where(s => s.AlbumId == albumId)
                .OrderBy(s => s.TrackNumber)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving songs for album ID: {AlbumId}", albumId);
            throw;
        }
    }

    public async Task<IEnumerable<Song>> GetByArtistIdAsync(Guid artistId)
    {
        try
        {
            _logger.LogInformation("Retrieving songs for artist ID: {ArtistId}", artistId);
            return await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .Where(s => s.ArtistId == artistId)
                .OrderBy(s => s.Album.ReleaseYear)
                .ThenBy(s => s.TrackNumber)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving songs for artist ID: {ArtistId}", artistId);
            throw;
        }
    }

    public async Task<IEnumerable<Song>> SearchByTitleAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation("Searching songs with term: {SearchTerm}", searchTerm);
            return await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .Where(s => s.Title.ToLower().Contains(searchTerm.ToLower()))
                .OrderBy(s => s.Title)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching songs with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
