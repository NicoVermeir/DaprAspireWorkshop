using Metalify.Bandcenter.Api.DTOs;
using Metalify.Bandcenter.Api.Models;
using Metalify.Bandcenter.Api.Repositories;

namespace Metalify.Bandcenter.Api.Services;

public class SongService : ISongService
{
    private readonly ISongRepository _songRepository;
    private readonly IAlbumRepository _albumRepository;

    public SongService(ISongRepository songRepository, IAlbumRepository albumRepository)
    {
        _songRepository = songRepository;
        _albumRepository = albumRepository;
    }

    public async Task<IEnumerable<SongSummaryDto>> GetAllSongsAsync()
    {
        var songs = await _songRepository.GetAllAsync();
        return songs.Select(MapToSongSummaryDto);
    }

    public async Task<SongDto?> GetSongByIdAsync(Guid id)
    {
        var song = await _songRepository.GetByIdAsync(id);
        return song == null ? null : MapToSongDto(song);
    }

    public async Task<IEnumerable<SongSummaryDto>> GetSongsByAlbumIdAsync(Guid albumId)
    {
        var songs = await _songRepository.GetByAlbumIdAsync(albumId);
        return songs.Select(MapToSongSummaryDto);
    }

    public async Task<IEnumerable<SongSummaryDto>> GetSongsByBandIdAsync(Guid bandId)
    {
        var songs = await _songRepository.GetByBandIdAsync(bandId);
        return songs.Select(MapToSongSummaryDto);
    }

    public async Task<IEnumerable<SongSummaryDto>> SearchSongsByTitleAsync(string searchTerm)
    {
        var songs = await _songRepository.SearchByTitleAsync(searchTerm);
        return songs.Select(MapToSongSummaryDto);
    }

    public async Task<IEnumerable<SongSummaryDto>> GetSongsByDurationRangeAsync(TimeSpan minDuration, TimeSpan maxDuration)
    {
        var songs = await _songRepository.GetByDurationRangeAsync(minDuration, maxDuration);
        return songs.Select(MapToSongSummaryDto);
    }

    public async Task<SongDto> CreateSongAsync(CreateSongDto createSongDto)
    {
        // Verify album exists
        var album = await _albumRepository.GetByIdAsync(createSongDto.AlbumId);
        if (album == null)
        {
            throw new ArgumentException($"Album with ID {createSongDto.AlbumId} does not exist.");
        }

        var song = new Song
        {
            Id = Guid.NewGuid(),
            Title = createSongDto.Title,
            TrackNumber = createSongDto.TrackNumber,
            Duration = createSongDto.Duration,
            Lyrics = createSongDto.Lyrics,
            Notes = createSongDto.Notes,
            AlbumId = createSongDto.AlbumId,
            ArtistId = createSongDto.BandId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdSong = await _songRepository.AddAsync(song);
        return MapToSongDto(createdSong);
    }

    public async Task<SongDto?> UpdateSongAsync(Guid id, UpdateSongDto updateSongDto)
    {
        var existingSong = await _songRepository.GetByIdAsync(id);
        if (existingSong == null)
            return null;

        if (!string.IsNullOrEmpty(updateSongDto.Title))
            existingSong.Title = updateSongDto.Title;
        if (updateSongDto.TrackNumber.HasValue)
            existingSong.TrackNumber = updateSongDto.TrackNumber.Value;
        if (updateSongDto.Duration.HasValue)
            existingSong.Duration = updateSongDto.Duration.Value;
        if (!string.IsNullOrEmpty(updateSongDto.Lyrics))
            existingSong.Lyrics = updateSongDto.Lyrics;
        if (!string.IsNullOrEmpty(updateSongDto.Notes))
            existingSong.Notes = updateSongDto.Notes;
        
        existingSong.UpdatedAt = DateTime.UtcNow;

        var updatedSong = await _songRepository.UpdateAsync(existingSong);
        return MapToSongDto(updatedSong);
    }

    public async Task<bool> DeleteSongAsync(Guid id)
    {
        return await _songRepository.DeleteAsync(id);
    }

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

    private static SongSummaryDto MapToSongSummaryDto(Song song) => new()
    {
        Id = song.Id,
        Title = song.Title,
        TrackNumber = song.TrackNumber,
        Duration = song.Duration,
        AlbumTitle = song.Album?.Title ?? string.Empty,
        BandName = song.Album?.Artist?.Name ?? string.Empty
    };
}
