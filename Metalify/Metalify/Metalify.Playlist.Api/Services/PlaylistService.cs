using Metalify.Playlist.Api.DTOs;
using Metalify.Playlist.Api.Models;
using Metalify.Playlist.Api.Repositories;

namespace Metalify.Playlist.Api.Services;

/// <summary>
/// Service interface for playlist business logic
/// </summary>
public interface IPlaylistService
{
    Task<IEnumerable<PlaylistSummaryDto>> GetAllPlaylistsAsync();
    Task<PlaylistDto?> GetPlaylistByIdAsync(Guid id);
    Task<IEnumerable<PlaylistSummaryDto>> GetPlaylistsByUserAsync(string userId);
    Task<IEnumerable<PlaylistSummaryDto>> GetPublicPlaylistsAsync();
    Task<IEnumerable<PlaylistSummaryDto>> SearchPlaylistsAsync(string searchTerm);
    Task<PlaylistDto> CreatePlaylistAsync(CreatePlaylistDto createDto);
    Task<PlaylistDto?> UpdatePlaylistAsync(Guid id, UpdatePlaylistDto updateDto);
    Task<bool> DeletePlaylistAsync(Guid id);
    Task<PlaylistDto?> AddSongToPlaylistAsync(Guid playlistId, AddSongToPlaylistDto addSongDto);
    Task<bool> RemoveSongFromPlaylistAsync(Guid playlistId, Guid songId);
    Task<PlaylistDto?> ReorderPlaylistAsync(Guid playlistId, ReorderPlaylistDto reorderDto);
}

/// <summary>
/// Service implementation for playlist business logic
/// </summary>
public class PlaylistService : IPlaylistService
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly ICatalogApiService _catalogApiService;
    private readonly ILogger<PlaylistService> _logger;

    public PlaylistService(
        IPlaylistRepository playlistRepository,
        ICatalogApiService catalogApiService,
        ILogger<PlaylistService> logger)
    {
        _playlistRepository = playlistRepository ?? throw new ArgumentNullException(nameof(playlistRepository));
        _catalogApiService = catalogApiService ?? throw new ArgumentNullException(nameof(catalogApiService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<PlaylistSummaryDto>> GetAllPlaylistsAsync()
    {
        _logger.LogInformation("Getting all playlists");
        var playlists = await _playlistRepository.GetAllAsync();
        return playlists.Select(MapToSummaryDto);
    }

    public async Task<PlaylistDto?> GetPlaylistByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting playlist by ID: {PlaylistId}", id);
        var playlist = await _playlistRepository.GetByIdAsync(id);
        
        if (playlist == null)
        {
            return null;
        }

        return await MapToDetailedDto(playlist);
    }

    public async Task<IEnumerable<PlaylistSummaryDto>> GetPlaylistsByUserAsync(string userId)
    {
        _logger.LogInformation("Getting playlists for user: {UserId}", userId);
        var playlists = await _playlistRepository.GetByCreatedByAsync(userId);
        return playlists.Select(MapToSummaryDto);
    }

    public async Task<IEnumerable<PlaylistSummaryDto>> GetPublicPlaylistsAsync()
    {
        _logger.LogInformation("Getting public playlists");
        var playlists = await _playlistRepository.GetPublicPlaylistsAsync();
        return playlists.Select(MapToSummaryDto);
    }

    public async Task<IEnumerable<PlaylistSummaryDto>> SearchPlaylistsAsync(string searchTerm)
    {
        _logger.LogInformation("Searching playlists with term: {SearchTerm}", searchTerm);
        var playlists = await _playlistRepository.SearchByNameAsync(searchTerm);
        return playlists.Select(MapToSummaryDto);
    }

    public async Task<PlaylistDto> CreatePlaylistAsync(CreatePlaylistDto createDto)
    {
        _logger.LogInformation("Creating playlist: {PlaylistName}", createDto.Name);
        
        var playlist = new Models.Playlist
        {
            Name = createDto.Name,
            Description = createDto.Description,
            CoverImageUrl = createDto.CoverImageUrl,
            IsPublic = createDto.IsPublic,
            CreatedBy = createDto.CreatedBy
        };

        var createdPlaylist = await _playlistRepository.CreateAsync(playlist);
        return await MapToDetailedDto(createdPlaylist);
    }

    public async Task<PlaylistDto?> UpdatePlaylistAsync(Guid id, UpdatePlaylistDto updateDto)
    {
        _logger.LogInformation("Updating playlist: {PlaylistId}", id);
        
        var existingPlaylist = await _playlistRepository.GetByIdAsync(id);
        if (existingPlaylist == null)
        {
            return null;
        }

        // Apply updates
        if (updateDto.Name != null) existingPlaylist.Name = updateDto.Name;
        if (updateDto.Description != null) existingPlaylist.Description = updateDto.Description;
        if (updateDto.CoverImageUrl != null) existingPlaylist.CoverImageUrl = updateDto.CoverImageUrl;
        if (updateDto.IsPublic.HasValue) existingPlaylist.IsPublic = updateDto.IsPublic.Value;

        var updatedPlaylist = await _playlistRepository.UpdateAsync(id, existingPlaylist);
        return updatedPlaylist != null ? await MapToDetailedDto(updatedPlaylist) : null;
    }

    public async Task<bool> DeletePlaylistAsync(Guid id)
    {
        _logger.LogInformation("Deleting playlist: {PlaylistId}", id);
        return await _playlistRepository.DeleteAsync(id);
    }

    public async Task<PlaylistDto?> AddSongToPlaylistAsync(Guid playlistId, AddSongToPlaylistDto addSongDto)
    {
        _logger.LogInformation("Adding song {SongId} to playlist {PlaylistId}", addSongDto.SongId, playlistId);
        
        // Verify song exists in catalog
        var song = await _catalogApiService.GetSongByIdAsync(addSongDto.SongId);
        if (song == null)
        {
            throw new ArgumentException($"Song with ID {addSongDto.SongId} not found in catalog");
        }

        await _playlistRepository.AddSongToPlaylistAsync(playlistId, addSongDto.SongId, addSongDto.Position);
        
        // Return updated playlist
        var updatedPlaylist = await _playlistRepository.GetByIdAsync(playlistId);
        return updatedPlaylist != null ? await MapToDetailedDto(updatedPlaylist) : null;
    }

    public async Task<bool> RemoveSongFromPlaylistAsync(Guid playlistId, Guid songId)
    {
        _logger.LogInformation("Removing song {SongId} from playlist {PlaylistId}", songId, playlistId);
        return await _playlistRepository.RemoveSongFromPlaylistAsync(playlistId, songId);
    }

    public async Task<PlaylistDto?> ReorderPlaylistAsync(Guid playlistId, ReorderPlaylistDto reorderDto)
    {
        _logger.LogInformation("Reordering playlist {PlaylistId}", playlistId);
        
        var reorderData = reorderDto.Items.Select(item => (item.ItemId, item.Position)).ToList();
        var success = await _playlistRepository.ReorderPlaylistAsync(playlistId, reorderData);
        
        if (!success)
        {
            return null;
        }

        // Return updated playlist
        var updatedPlaylist = await _playlistRepository.GetByIdAsync(playlistId);
        return updatedPlaylist != null ? await MapToDetailedDto(updatedPlaylist) : null;
    }

    private static PlaylistSummaryDto MapToSummaryDto(Models.Playlist playlist)
    {
        return new PlaylistSummaryDto
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Description = playlist.Description,
            CoverImageUrl = playlist.CoverImageUrl,
            IsPublic = playlist.IsPublic,
            CreatedBy = playlist.CreatedBy,
            CreatedAt = playlist.CreatedAt,
            UpdatedAt = playlist.UpdatedAt,
            SongCount = playlist.PlaylistItems.Count,
            TotalDuration = TimeSpan.Zero // Will be calculated when we have song details
        };
    }

    private async Task<PlaylistDto> MapToDetailedDto(Models.Playlist playlist)
    {
        var playlistItems = new List<PlaylistItemDto>();
        
        if (playlist.PlaylistItems.Any())
        {
            // Get song details from catalog API
            var songIds = playlist.PlaylistItems.Select(pi => pi.SongId).ToList();
            var songs = await _catalogApiService.GetSongsByIdsAsync(songIds);
            var songDict = songs.ToDictionary(s => s.Id);

            playlistItems = playlist.PlaylistItems
                .OrderBy(pi => pi.Position)
                .Select(pi =>
                {
                    var song = songDict.GetValueOrDefault(pi.SongId);
                    return new PlaylistItemDto
                    {
                        Id = pi.Id,
                        SongId = pi.SongId,
                        SongTitle = song?.Title ?? "Unknown Song",
                        ArtistName = song?.ArtistName ?? "Unknown Artist",
                        AlbumTitle = song?.AlbumTitle ?? "Unknown Album",
                        Duration = song?.Duration ?? TimeSpan.Zero,
                        Position = pi.Position,
                        AddedAt = pi.AddedAt
                    };
                })
                .ToList();
        }

        var totalDuration = playlistItems.Aggregate(TimeSpan.Zero, (sum, item) => sum.Add(item.Duration));

        return new PlaylistDto
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Description = playlist.Description,
            CoverImageUrl = playlist.CoverImageUrl,
            IsPublic = playlist.IsPublic,
            CreatedBy = playlist.CreatedBy,
            CreatedAt = playlist.CreatedAt,
            UpdatedAt = playlist.UpdatedAt,
            SongCount = playlistItems.Count,
            TotalDuration = totalDuration,
            Songs = playlistItems
        };
    }
}
