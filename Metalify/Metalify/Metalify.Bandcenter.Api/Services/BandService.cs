using Metalify.Bandcenter.Api.DTOs;
using Metalify.Bandcenter.Api.Models;
using Metalify.Bandcenter.Api.Repositories;
using System.Threading;
using Dapr.Client;

namespace Metalify.Bandcenter.Api.Services;

public class BandService(IBandRepository bandRepository, DaprClient daprClient) : IBandService
{
    const string PUBSUB_NAME = "pubsub";
    const string TOPIC_NAME = "artist-changed";

    public async Task<IEnumerable<BandSummaryDto>> GetAllBandsAsync()
    {
        var bands = await bandRepository.GetAllAsync();
        return bands.Select(MapToBandSummaryDto);
    }

    public async Task<BandDto?> GetBandByIdAsync(Guid id)
    {
        var band = await bandRepository.GetByIdAsync(id);
        return band == null ? null : MapToBandDto(band);
    }

    public async Task<BandDto?> GetBandWithAlbumsAsync(Guid id)
    {
        var band = await bandRepository.GetByIdWithAlbumsAsync(id);
        return band == null ? null : MapToBandDto(band);
    }

    public async Task<IEnumerable<BandSummaryDto>> SearchBandsByNameAsync(string searchTerm)
    {
        var bands = await bandRepository.SearchByNameAsync(searchTerm);
        return bands.Select(MapToBandSummaryDto);
    }

    public async Task<IEnumerable<BandSummaryDto>> GetBandsByGenreAsync(string genre)
    {
        var bands = await bandRepository.GetByGenreAsync(genre);
        return bands.Select(MapToBandSummaryDto);
    }

    public async Task<IEnumerable<BandSummaryDto>> GetBandsByCountryAsync(string country)
    {
        var bands = await bandRepository.GetByCountryAsync(country);
        return bands.Select(MapToBandSummaryDto);
    }

    public async Task<BandDto> CreateBandAsync(CreateBandDto createBandDto)
    {
        var band = new Artist
        {
            Id = Guid.NewGuid(),
            Name = createBandDto.Name,
            Country = createBandDto.Country,
            Location = createBandDto.Location,
            Status = createBandDto.Status,
            FormedYear = createBandDto.FormedYear,
            Genres = createBandDto.Genre,
            Themes = createBandDto.Themes,
            Label = createBandDto.Label,
            YearsActive = createBandDto.YearsActive,
            LogoUrl = createBandDto.LogoUrl,
            ImageUrl = createBandDto.PhotoUrl,
            Biography = createBandDto.Biography,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdBand = await bandRepository.AddAsync(band);
        return MapToBandDto(createdBand);
    }

    public async Task<BandDto?> UpdateBandAsync(Guid id, UpdateBandDto updateBandDto)
    {
        var existingBand = await bandRepository.GetByIdAsync(id);
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
            existingBand.Genres = updateBandDto.Genre;
        if (!string.IsNullOrEmpty(updateBandDto.Themes))
            existingBand.Themes = updateBandDto.Themes;
        if (!string.IsNullOrEmpty(updateBandDto.Label))
            existingBand.Label = updateBandDto.Label;
        if (!string.IsNullOrEmpty(updateBandDto.YearsActive))
            existingBand.YearsActive = updateBandDto.YearsActive;
        if (!string.IsNullOrEmpty(updateBandDto.LogoUrl))
            existingBand.LogoUrl = updateBandDto.LogoUrl;
        if (!string.IsNullOrEmpty(updateBandDto.PhotoUrl))
            existingBand.ImageUrl = updateBandDto.PhotoUrl;
        if (!string.IsNullOrEmpty(updateBandDto.Biography))
            existingBand.Biography = updateBandDto.Biography;
        
        existingBand.UpdatedAt = DateTime.UtcNow;

        var updatedBand = await bandRepository.UpdateAsync(existingBand);

        var mappedDto = MapToBandDto(updatedBand);
        await PublishMessage(mappedDto);

        return mappedDto;
    }

    private async Task PublishMessage(BandDto artist)
    {
        await daprClient.PublishEventAsync(PUBSUB_NAME, TOPIC_NAME, artist);
    }

    public async Task<bool> DeleteBandAsync(Guid id)
    {
        return await bandRepository.DeleteAsync(id);
    }

    private static BandDto MapToBandDto(Artist band) => new()
    {
        Id = band.Id,
        Name = band.Name,
        Country = band.Country,
        Location = band.Location,
        Status = band.Status,
        FormedYear = band.FormedYear,
        Genre = band.Genres,
        Themes = band.Themes,
        Label = band.Label,
        YearsActive = band.YearsActive,
        LogoUrl = band.LogoUrl,
        PhotoUrl = band.ImageUrl,
        Biography = band.Biography,
        CreatedAt = band.CreatedAt,
        UpdatedAt = band.UpdatedAt,
        Albums = band.Albums?.Select(MapToAlbumSummaryDto).ToList() ?? new List<AlbumSummaryDto>()
    };

    private static BandSummaryDto MapToBandSummaryDto(Artist band) => new()
    {
        Id = band.Id,
        Name = band.Name,
        Country = band.Country,
        Genre = band.Genres,
        FormedYear = band.FormedYear,
        Status = band.Status,
        LogoUrl = band.LogoUrl,
        PhotoUrl = band.ImageUrl,
        AlbumCount = band.Albums?.Count ?? 0
    };

    private static AlbumSummaryDto MapToAlbumSummaryDto(Album album) => new()
    {
        Id = album.Id,
        Title = album.Title,
        AlbumType = album.AlbumType,
        ReleaseYear = album.ReleaseYear,
        CoverImageUrl = album.CoverImageUrl,
        BandName = album.Artist?.Name ?? string.Empty,
        TotalDuration = album.TotalDuration,
        SongCount = album.Songs?.Count ?? 0
    };
}
