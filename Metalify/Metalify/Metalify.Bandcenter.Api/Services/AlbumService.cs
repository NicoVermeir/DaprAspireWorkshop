using Metalify.Bandcenter.Api.DTOs;
using Metalify.Bandcenter.Api.Models;
using Metalify.Bandcenter.Api.Repositories;

namespace Metalify.Bandcenter.Api.Services;

public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _albumRepository;
    private readonly IBandRepository _bandRepository;

    public AlbumService(IAlbumRepository albumRepository, IBandRepository bandRepository)
    {
        _albumRepository = albumRepository;
        _bandRepository = bandRepository;
    }

    public async Task<IEnumerable<AlbumSummaryDto>> GetAllAlbumsAsync()
    {
        var albums = await _albumRepository.GetAllAsync();
        return albums.Select(MapToAlbumSummaryDto);
    }

    public async Task<AlbumDto?> GetAlbumByIdAsync(Guid id)
    {
        var album = await _albumRepository.GetByIdAsync(id);
        return album == null ? null : MapToAlbumDto(album);
    }

    public async Task<AlbumDto?> GetAlbumWithSongsAsync(Guid id)
    {
        var album = await _albumRepository.GetByIdWithSongsAsync(id);
        return album == null ? null : MapToAlbumDto(album);
    }

    public async Task<IEnumerable<AlbumSummaryDto>> GetAlbumsByBandIdAsync(Guid bandId)
    {
        var albums = await _albumRepository.GetByBandIdAsync(bandId);
        return albums.Select(MapToAlbumSummaryDto);
    }

    public async Task<IEnumerable<AlbumSummaryDto>> SearchAlbumsByTitleAsync(string searchTerm)
    {
        var albums = await _albumRepository.SearchByTitleAsync(searchTerm);
        return albums.Select(MapToAlbumSummaryDto);
    }

    public async Task<IEnumerable<AlbumSummaryDto>> GetAlbumsByAlbumTypeAsync(string albumType)
    {
        var albums = await _albumRepository.GetByAlbumTypeAsync(albumType);
        return albums.Select(MapToAlbumSummaryDto);
    }

    public async Task<IEnumerable<AlbumSummaryDto>> GetAlbumsByYearAsync(int year)
    {
        var albums = await _albumRepository.GetByYearAsync(year);
        return albums.Select(MapToAlbumSummaryDto);
    }

    public async Task<IEnumerable<AlbumSummaryDto>> GetAlbumsByYearRangeAsync(int startYear, int endYear)
    {
        var albums = await _albumRepository.GetByYearRangeAsync(startYear, endYear);
        return albums.Select(MapToAlbumSummaryDto);
    }

    public async Task<AlbumDto> CreateAlbumAsync(CreateAlbumDto createAlbumDto)
    {
        // Verify band exists
        if (!await _bandRepository.ExistsAsync(createAlbumDto.BandId))
        {
            throw new ArgumentException($"Band with ID {createAlbumDto.BandId} does not exist.");
        }

        var album = new Album
        {
            Id = Guid.NewGuid(),
            Title = createAlbumDto.Title,
            AlbumType = createAlbumDto.AlbumType,
            ReleaseYear = createAlbumDto.ReleaseYear,
            CoverImageUrl = createAlbumDto.CoverImageUrl,
            Label = createAlbumDto.Label,
            CatalogNumber = createAlbumDto.CatalogNumber,
            Format = createAlbumDto.Format,
            Notes = createAlbumDto.Notes,
            ArtistId = createAlbumDto.BandId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdAlbum = await _albumRepository.AddAsync(album);
        return MapToAlbumDto(createdAlbum);
    }

    public async Task<AlbumDto?> UpdateAlbumAsync(Guid id, UpdateAlbumDto updateAlbumDto)
    {
        var existingAlbum = await _albumRepository.GetByIdAsync(id);
        if (existingAlbum == null)
            return null;

        if (!string.IsNullOrEmpty(updateAlbumDto.Title))
            existingAlbum.Title = updateAlbumDto.Title;
        if (!string.IsNullOrEmpty(updateAlbumDto.AlbumType))
            existingAlbum.AlbumType = updateAlbumDto.AlbumType;
        if (updateAlbumDto.ReleaseYear.HasValue)
            existingAlbum.ReleaseYear = updateAlbumDto.ReleaseYear.Value;
        if (!string.IsNullOrEmpty(updateAlbumDto.CoverImageUrl))
            existingAlbum.CoverImageUrl = updateAlbumDto.CoverImageUrl;
        if (!string.IsNullOrEmpty(updateAlbumDto.Label))
            existingAlbum.Label = updateAlbumDto.Label;
        if (!string.IsNullOrEmpty(updateAlbumDto.CatalogNumber))
            existingAlbum.CatalogNumber = updateAlbumDto.CatalogNumber;
        if (!string.IsNullOrEmpty(updateAlbumDto.Format))
            existingAlbum.Format = updateAlbumDto.Format;
        if (!string.IsNullOrEmpty(updateAlbumDto.Notes))
            existingAlbum.Notes = updateAlbumDto.Notes;
        
        existingAlbum.UpdatedAt = DateTime.UtcNow;

        var updatedAlbum = await _albumRepository.UpdateAsync(existingAlbum);
        return MapToAlbumDto(updatedAlbum);
    }

    public async Task<bool> DeleteAlbumAsync(Guid id)
    {
        return await _albumRepository.DeleteAsync(id);
    }

    private static AlbumDto MapToAlbumDto(Album album) => new()
    {
        Id = album.Id,
        Title = album.Title,
        AlbumType = album.AlbumType,
        ReleaseYear = album.ReleaseYear,
        CoverImageUrl = album.CoverImageUrl,
        Label = album.Label,
        CatalogNumber = album.CatalogNumber,
        Format = album.Format,
        TotalDuration = album.TotalDuration,
        Notes = album.Notes,
        BandId = album.ArtistId,
        BandName = album.Artist?.Name ?? string.Empty,
        Songs = album.Songs?.Select(MapToSongDto).ToList() ?? new List<SongDto>()
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

    private static SongDto MapToSongDto(Song song) => new()
    {
        Id = song.Id,
        Title = song.Title,
        TrackNumber = song.TrackNumber,
        Duration = song.Duration,
        Lyrics = song.Lyrics,
        Notes = song.Notes,
        AlbumId = song.AlbumId,
        AlbumTitle = song.Album?.Title ?? string.Empty,
        BandId = song.ArtistId,
        BandName = song.Album?.Artist?.Name ?? string.Empty
    };
}
