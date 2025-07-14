using Microsoft.EntityFrameworkCore;
using Metalify.Bandcenter.Api.Data;
using Metalify.Bandcenter.Api.Models;

namespace Metalify.Bandcenter.Api.Repositories;

public class BandRepository : IBandRepository
{
    private readonly CatalogDbContext _context;

    public BandRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Artist>> GetAllAsync()
    {
        return await _context.Bands
            .OrderBy(b => b.Name)
            .ToListAsync();
    }    public async Task<Artist?> GetByIdAsync(Guid id)
    {
        return await _context.Bands
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Artist?> GetByIdWithAlbumsAsync(Guid id)
    {
        return await _context.Bands
            .Include(b => b.Albums)
            .ThenInclude(a => a.Songs)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Artist?> GetByNameAsync(string name)
    {
        return await _context.Bands
            .FirstOrDefaultAsync(b => b.Name.ToLower() == name.ToLower());
    }

    public async Task<IEnumerable<Artist>> SearchByNameAsync(string searchTerm)
    {
        return await _context.Bands
            .Where(b => b.Name.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Artist>> GetByGenreAsync(string genre)
    {
        return await _context.Bands
            .Where(b => b.Genres.ToLower().Contains(genre.ToLower()))
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Artist>> GetByCountryAsync(string country)
    {
        return await _context.Bands
            .Where(b => b.Country.ToLower() == country.ToLower())
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<Artist> AddAsync(Artist band)
    {
        _context.Bands.Add(band);
        await _context.SaveChangesAsync();
        return band;
    }

    public async Task<Artist> UpdateAsync(Artist band)
    {
        _context.Entry(band).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return band;
    }    public async Task<bool> DeleteAsync(Guid id)
    {
        var band = await _context.Bands.FindAsync(id);
        if (band == null)
            return false;

        _context.Bands.Remove(band);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Bands.AnyAsync(b => b.Id == id);
    }
}
