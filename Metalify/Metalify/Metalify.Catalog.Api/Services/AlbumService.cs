using Metalify.Catalog.Api.DTOs;
using Metalify.Catalog.Api.Models;
using Metalify.Catalog.Api.Repositories;

namespace Metalify.Catalog.Api.Services;

/// <summary>
/// Service implementation for Album business logic
/// </summary>
public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _albumRepository;
    private readonly ILogger<AlbumService> _logger;

    public AlbumService(IAlbumRepository albumRepository, ILogger<AlbumService> logger)
    {
        _albumRepository = albumRepository ?? throw new ArgumentNullException(nameof(albumRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<AlbumSummaryDto>> GetAllAlbumsAsync()
    {
        _logger.LogInformation("Getting all albums");
        var albums = await _albumRepository.GetAllAsync();
        return albums.Select(MapToSummaryDto);
    }

    public async Task<AlbumDto?> GetAlbumByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting album by ID: {AlbumId}", id);
        var album = await _albumRepository.GetByIdAsync(id);
        return album != null ? MapToDto(album) : null;
    }

    public async Task<IEnumerable<AlbumSummaryDto>> GetAlbumsByArtistIdAsync(Guid artistId)
    {
        _logger.LogInformation("Getting albums by artist ID: {ArtistId}", artistId);
        var albums = await _albumRepository.GetByArtistIdAsync(artistId);
        return albums.Select(MapToSummaryDto);
    }

    public async Task<IEnumerable<AlbumSummaryDto>> GetAlbumsByYearAsync(int year)
    {
        _logger.LogInformation("Getting albums by year: {Year}", year);
        var albums = await _albumRepository.GetByYearAsync(year);
        return albums.Select(MapToSummaryDto);
    }

    public async Task<IEnumerable<AlbumSummaryDto>> SearchAlbumsAsync(string searchTerm)
    {
        _logger.LogInformation("Searching albums with term: {SearchTerm}", searchTerm);
        var albums = await _albumRepository.SearchByTitleAsync(searchTerm);
        return albums.Select(MapToSummaryDto);
    }

    private static AlbumDto MapToDto(Album album)
    {
        return new AlbumDto
        {
            Id = album.Id,
            Title = album.Title,
            CoverImageUrl = album.CoverImageUrl,
            ReleaseYear = album.ReleaseYear,
            ArtistId = album.ArtistId,
            ArtistName = album.Artist.Name,
            Songs = album.Songs.Select(song => new SongDto
            {
                Id = song.Id,
                Title = song.Title,
                TrackNumber = song.TrackNumber,
                Duration = song.Duration,
                AlbumId = song.AlbumId,
                AlbumTitle = album.Title,
                ArtistId = song.ArtistId,
                ArtistName = album.Artist.Name,
                AudioUrl = song.AudioUrl
            }).OrderBy(s => s.TrackNumber).ToList()
        };
    }

    private static AlbumSummaryDto MapToSummaryDto(Album album)
    {
        return new AlbumSummaryDto
        {
            Id = album.Id,
            Title = album.Title,
            CoverImageUrl = album.CoverImageUrl,
            ReleaseYear = album.ReleaseYear,
            ArtistId = album.ArtistId,
            ArtistName = album.Artist.Name,
            SongCount = album.Songs.Count
        };
    }
}
