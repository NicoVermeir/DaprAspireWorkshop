using Metalify.Bandcenter.Api.DTOs;
using Metalify.Bandcenter.Api.Models;
using Metalify.Bandcenter.Api.Repositories;

namespace Metalify.Bandcenter.Api.Services;

public class BandService : IBandService
{
    private readonly IBandRepository _bandRepository;

    public BandService(IBandRepository bandRepository)
    {
        _bandRepository = bandRepository;
    }

    public async Task<IEnumerable<BandSummaryDto>> GetAllBandsAsync()
    {
        var bands = await _bandRepository.GetAllAsync();
        return bands.Select(MapToBandSummaryDto);
    }

    public async Task<BandDto?> GetBandByIdAsync(Guid id)
    {
        var band = await _bandRepository.GetByIdAsync(id);
        return band == null ? null : MapToBandDto(band);
    }

    public async Task<BandDto?> GetBandWithAlbumsAsync(Guid id)
    {
        var band = await _bandRepository.GetByIdWithAlbumsAsync(id);
        return band == null ? null : MapToBandDto(band);
    }

    public async Task<IEnumerable<BandSummaryDto>> SearchBandsByNameAsync(string searchTerm)
    {
        var bands = await _bandRepository.SearchByNameAsync(searchTerm);
        return bands.Select(MapToBandSummaryDto);
    }

    public async Task<IEnumerable<BandSummaryDto>> GetBandsByGenreAsync(string genre)
    {
        var bands = await _bandRepository.GetByGenreAsync(genre);
        return bands.Select(MapToBandSummaryDto);
    }

    public async Task<IEnumerable<BandSummaryDto>> GetBandsByCountryAsync(string country)
    {
        var bands = await _bandRepository.GetByCountryAsync(country);
        return bands.Select(MapToBandSummaryDto);
    }

    public async Task<BandDto> CreateBandAsync(CreateBandDto createBandDto)
    {
        var band = new Band
        {
            Id = Guid.NewGuid(),
            Name = createBandDto.Name,
            Country = createBandDto.Country,
            Location = createBandDto.Location,
            Status = createBandDto.Status,
            FormedYear = createBandDto.FormedYear,
            Genre = createBandDto.Genre,
            Themes = createBandDto.Themes,
            Label = createBandDto.Label,
            YearsActive = createBandDto.YearsActive,
            LogoUrl = createBandDto.LogoUrl,
            PhotoUrl = createBandDto.PhotoUrl,
            Biography = createBandDto.Biography,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdBand = await _bandRepository.AddAsync(band);
        return MapToBandDto(createdBand);
    }

    public async Task<BandDto?> UpdateBandAsync(Guid id, UpdateBandDto updateBandDto)
    {
        var existingBand = await _bandRepository.GetByIdAsync(id);
        if (existingBand == null)
            return null;

        if (!string.IsNullOrEmpty(updateBandDto.Name))
            existingBand.Name = updateBandDto.Name;
        if (!string.IsNullOrEmpty(updateBandDto.Country))
            existingBand.Country = updateBandDto.Country;
        if (!string.IsNullOrEmpty(updateBandDto.Location))
            existingBand.Location = updateBandDto.Location;
        if (!string.IsNullOrEmpty(updateBandDto.Status))
            existingBand.Status = updateBandDto.Status;
        if (updateBandDto.FormedYear.HasValue)
            existingBand.FormedYear = updateBandDto.FormedYear.Value;
        if (!string.IsNullOrEmpty(updateBandDto.Genre))
            existingBand.Genre = updateBandDto.Genre;
        if (!string.IsNullOrEmpty(updateBandDto.Themes))
            existingBand.Themes = updateBandDto.Themes;
        if (!string.IsNullOrEmpty(updateBandDto.Label))
            existingBand.Label = updateBandDto.Label;
        if (!string.IsNullOrEmpty(updateBandDto.YearsActive))
            existingBand.YearsActive = updateBandDto.YearsActive;
        if (!string.IsNullOrEmpty(updateBandDto.LogoUrl))
            existingBand.LogoUrl = updateBandDto.LogoUrl;
        if (!string.IsNullOrEmpty(updateBandDto.PhotoUrl))
            existingBand.PhotoUrl = updateBandDto.PhotoUrl;
        if (!string.IsNullOrEmpty(updateBandDto.Biography))
            existingBand.Biography = updateBandDto.Biography;
        
        existingBand.UpdatedAt = DateTime.UtcNow;

        var updatedBand = await _bandRepository.UpdateAsync(existingBand);
        return MapToBandDto(updatedBand);
    }

    public async Task<bool> DeleteBandAsync(Guid id)
    {
        return await _bandRepository.DeleteAsync(id);
    }

    private static BandDto MapToBandDto(Band band) => new()
    {
        Id = band.Id,
        Name = band.Name,
        Country = band.Country,
        Location = band.Location,
        Status = band.Status,
        FormedYear = band.FormedYear,
        Genre = band.Genre,
        Themes = band.Themes,
        Label = band.Label,
        YearsActive = band.YearsActive,
        LogoUrl = band.LogoUrl,
        PhotoUrl = band.PhotoUrl,
        Biography = band.Biography,
        CreatedAt = band.CreatedAt,
        UpdatedAt = band.UpdatedAt,
        Albums = band.Albums?.Select(MapToAlbumSummaryDto).ToList() ?? new List<AlbumSummaryDto>()
    };

    private static BandSummaryDto MapToBandSummaryDto(Band band) => new()
    {
        Id = band.Id,
        Name = band.Name,
        Country = band.Country,
        Genre = band.Genre,
        FormedYear = band.FormedYear,
        Status = band.Status,
        LogoUrl = band.LogoUrl,
        PhotoUrl = band.PhotoUrl,
        AlbumCount = band.Albums?.Count ?? 0
    };

    private static AlbumSummaryDto MapToAlbumSummaryDto(Album album) => new()
    {
        Id = album.Id,
        Title = album.Title,
        AlbumType = album.AlbumType,
        ReleaseYear = album.ReleaseYear,
        CoverImageUrl = album.CoverImageUrl,
        BandName = album.Band?.Name ?? string.Empty,
        TotalDuration = album.TotalDuration,
        SongCount = album.Songs?.Count ?? 0
    };
}
