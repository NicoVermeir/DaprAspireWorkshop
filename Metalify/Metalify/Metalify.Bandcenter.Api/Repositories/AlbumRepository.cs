using Microsoft.EntityFrameworkCore;
using Metalify.Bandcenter.Api.Data;
using Metalify.Bandcenter.Api.Models;

namespace Metalify.Bandcenter.Api.Repositories;

public class AlbumRepository : IAlbumRepository
{
    private readonly CatalogDbContext _context;

    public AlbumRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Album>> GetAllAsync()
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .OrderBy(a => a.Title)
            .ToListAsync();
    }    public async Task<Album?> GetByIdAsync(Guid id)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Album?> GetByIdWithSongsAsync(Guid id)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Songs)
            .FirstOrDefaultAsync(a => a.Id == id);
    }    public async Task<IEnumerable<Album>> GetByBandIdAsync(Guid bandId)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Where(a => a.ArtistId == bandId)
            .OrderBy(a => a.ReleaseYear)
            .ThenBy(a => a.Title)
            .ToListAsync();
    }

    public async Task<IEnumerable<Album>> SearchByTitleAsync(string searchTerm)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Where(a => a.Title.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(a => a.Title)
            .ToListAsync();
    }    public async Task<IEnumerable<Album>> GetByAlbumTypeAsync(string albumType)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Where(a => a.AlbumType.ToLower().Contains(albumType.ToLower()))
            .OrderBy(a => a.Title)
            .ToListAsync();
    }

    public async Task<IEnumerable<Album>> GetByYearAsync(int year)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Where(a => a.ReleaseYear == year)
            .OrderBy(a => a.Title)
            .ToListAsync();
    }

    public async Task<IEnumerable<Album>> GetByYearRangeAsync(int startYear, int endYear)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Where(a => a.ReleaseYear >= startYear && a.ReleaseYear <= endYear)
            .OrderBy(a => a.ReleaseYear)
            .ThenBy(a => a.Title)
            .ToListAsync();
    }

    public async Task<Album> AddAsync(Album album)
    {
        _context.Albums.Add(album);
        await _context.SaveChangesAsync();
        return album;
    }

    public async Task<Album> UpdateAsync(Album album)
    {
        _context.Entry(album).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return album;
    }    public async Task<bool> DeleteAsync(Guid id)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album == null)
            return false;

        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Albums.AnyAsync(a => a.Id == id);
    }
}
