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

    public async Task<IEnumerable<Band>> GetAllAsync()
    {
        return await _context.Bands
            .OrderBy(b => b.Name)
            .ToListAsync();
    }    public async Task<Band?> GetByIdAsync(Guid id)
    {
        return await _context.Bands
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Band?> GetByIdWithAlbumsAsync(Guid id)
    {
        return await _context.Bands
            .Include(b => b.Albums)
            .ThenInclude(a => a.Songs)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Band?> GetByNameAsync(string name)
    {
        return await _context.Bands
            .FirstOrDefaultAsync(b => b.Name.ToLower() == name.ToLower());
    }

    public async Task<IEnumerable<Band>> SearchByNameAsync(string searchTerm)
    {
        return await _context.Bands
            .Where(b => b.Name.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Band>> GetByGenreAsync(string genre)
    {
        return await _context.Bands
            .Where(b => b.Genre.ToLower().Contains(genre.ToLower()))
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Band>> GetByCountryAsync(string country)
    {
        return await _context.Bands
            .Where(b => b.Country.ToLower() == country.ToLower())
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<Band> AddAsync(Band band)
    {
        _context.Bands.Add(band);
        await _context.SaveChangesAsync();
        return band;
    }

    public async Task<Band> UpdateAsync(Band band)
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
