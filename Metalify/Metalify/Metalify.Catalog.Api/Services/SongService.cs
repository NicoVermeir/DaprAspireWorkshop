using Metalify.Catalog.Api.DTOs;
using Metalify.Catalog.Api.Models;
using Metalify.Catalog.Api.Repositories;

namespace Metalify.Catalog.Api.Services;

/// <summary>
/// Service implementation for Song business logic
/// </summary>
public class SongService : ISongService
{
    private readonly ISongRepository _songRepository;
    private readonly ILogger<SongService> _logger;

    public SongService(ISongRepository songRepository, ILogger<SongService> logger)
    {
        _songRepository = songRepository ?? throw new ArgumentNullException(nameof(songRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<SongDto>> GetAllSongsAsync()
    {
        _logger.LogInformation("Getting all songs");
        var songs = await _songRepository.GetAllAsync();
        return songs.Select(MapToDto);
    }

    public async Task<SongDto?> GetSongByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting song by ID: {SongId}", id);
        var song = await _songRepository.GetByIdAsync(id);
        return song != null ? MapToDto(song) : null;
    }

    public async Task<IEnumerable<SongDto>> GetSongsByAlbumIdAsync(Guid albumId)
    {
        _logger.LogInformation("Getting songs by album ID: {AlbumId}", albumId);
        var songs = await _songRepository.GetByAlbumIdAsync(albumId);
        return songs.Select(MapToDto);
    }

    public async Task<IEnumerable<SongDto>> GetSongsByArtistIdAsync(Guid artistId)
    {
        _logger.LogInformation("Getting songs by artist ID: {ArtistId}", artistId);
        var songs = await _songRepository.GetByArtistIdAsync(artistId);
        return songs.Select(MapToDto);
    }

    public async Task<IEnumerable<SongDto>> SearchSongsAsync(string searchTerm)
    {
        _logger.LogInformation("Searching songs with term: {SearchTerm}", searchTerm);
        var songs = await _songRepository.SearchByTitleAsync(searchTerm);
        return songs.Select(MapToDto);
    }    private static SongDto MapToDto(Song song)
    {
        return new SongDto
        {
            Id = song.Id,
            Title = song.Title,
            TrackNumber = song.TrackNumber,
            Duration = song.Duration,
            AlbumId = song.AlbumId,
            AlbumTitle = song.Album.Title,
            AlbumCoverImageUrl = song.Album.CoverImageUrl,
            ArtistId = song.ArtistId,
            ArtistName = song.Artist.Name,
            AudioUrl = song.AudioUrl
        };
    }
}
