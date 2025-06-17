using Microsoft.EntityFrameworkCore;
using Metalify.Bandcenter.Api.Data;
using Metalify.Bandcenter.Api.Models;

namespace Metalify.Bandcenter.Api.Repositories;

public class SongRepository : ISongRepository
{
    private readonly CatalogDbContext _context;

    public SongRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Song>> GetAllAsync()
    {
        return await _context.Songs
            .Include(s => s.Album)
            .ThenInclude(a => a.Band)
            .OrderBy(s => s.Title)
            .ToListAsync();
    }    public async Task<Song?> GetByIdAsync(Guid id)
    {
        return await _context.Songs
            .Include(s => s.Album)
            .ThenInclude(a => a.Band)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Song>> GetByAlbumIdAsync(Guid albumId)
    {
        return await _context.Songs
            .Include(s => s.Album)
            .ThenInclude(a => a.Band)
            .Where(s => s.AlbumId == albumId)
            .OrderBy(s => s.TrackNumber)
            .ThenBy(s => s.Title)
            .ToListAsync();
    }    public async Task<IEnumerable<Song>> GetByBandIdAsync(Guid bandId)
    {
        return await _context.Songs
            .Include(s => s.Album)
            .ThenInclude(a => a.Band)
            .Where(s => s.BandId == bandId)
            .OrderBy(s => s.Album.ReleaseYear)
            .ThenBy(s => s.TrackNumber)
            .ToListAsync();
    }    public async Task<IEnumerable<Song>> SearchByTitleAsync(string searchTerm)
    {
        return await _context.Songs
            .Include(s => s.Album)
            .ThenInclude(a => a.Band)
            .Where(s => s.Title.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(s => s.Title)
            .ToListAsync();
    }

    public async Task<IEnumerable<Song>> GetByDurationRangeAsync(TimeSpan minDuration, TimeSpan maxDuration)
    {
        return await _context.Songs
            .Include(s => s.Album)
            .ThenInclude(a => a.Band)
            .Where(s => s.Duration >= minDuration && s.Duration <= maxDuration)
            .OrderBy(s => s.Duration)
            .ToListAsync();
    }

    public async Task<Song> AddAsync(Song song)
    {
        _context.Songs.Add(song);
        await _context.SaveChangesAsync();
        return song;
    }

    public async Task<Song> UpdateAsync(Song song)
    {
        _context.Entry(song).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return song;
    }    public async Task<bool> DeleteAsync(Guid id)
    {
        var song = await _context.Songs.FindAsync(id);
        if (song == null)
            return false;

        _context.Songs.Remove(song);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Songs.AnyAsync(s => s.Id == id);
    }
}
