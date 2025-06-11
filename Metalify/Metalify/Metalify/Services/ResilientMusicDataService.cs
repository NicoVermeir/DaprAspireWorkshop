using Metalify.Client.Models;
using Metalify.Services.Interfaces;

namespace Metalify.Services;

/// <summary>
/// Resilient music data service that provides fallback capabilities
/// when the primary API service fails
/// </summary>
public class ResilientMusicDataService(
    ApiMusicDataService primaryService,
    FakeMusicDataService fallbackService,
    ILogger<ResilientMusicDataService> logger)
    : IMusicDataService
{
    public async Task<IEnumerable<Artist>> GetArtistsAsync()
    {
        try
        {
            return await primaryService.GetArtistsAsync();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Primary service failed for GetArtistsAsync, falling back to fake data");
            return await fallbackService.GetArtistsAsync();
        }
    }

    public async Task<IEnumerable<Album>> GetAlbumsAsync()
    {
        try
        {
            return await primaryService.GetAlbumsAsync();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Primary service failed for GetAlbumsAsync, falling back to fake data");
            return await fallbackService.GetAlbumsAsync();
        }
    }

    public async Task<IEnumerable<Song>> GetSongsAsync()
    {
        try
        {
            return await primaryService.GetSongsAsync();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Primary service failed for GetSongsAsync, falling back to fake data");
            return await fallbackService.GetSongsAsync();
        }
    }

    public async Task<IEnumerable<Playlist>> GetPlaylistsAsync()
    {
        try
        {
            return await primaryService.GetPlaylistsAsync();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Primary service failed for GetPlaylistsAsync, falling back to fake data");
            return await fallbackService.GetPlaylistsAsync();
        }
    }

    public async Task<Artist?> GetArtistByIdAsync(Guid id)
    {
        try
        {
            return await primaryService.GetArtistByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Primary service failed for GetArtistByIdAsync({ArtistId}), falling back to fake data", id);
            return await fallbackService.GetArtistByIdAsync(id);
        }
    }

    public async Task<Album?> GetAlbumByIdAsync(Guid id)
    {
        try
        {
            return await primaryService.GetAlbumByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Primary service failed for GetAlbumByIdAsync({AlbumId}), falling back to fake data", id);
            return await fallbackService.GetAlbumByIdAsync(id);
        }
    }

    public async Task<Song?> GetSongByIdAsync(Guid id)
    {
        try
        {
            return await primaryService.GetSongByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Primary service failed for GetSongByIdAsync({SongId}), falling back to fake data", id);
            return await fallbackService.GetSongByIdAsync(id);
        }
    }

    public async Task<Playlist?> GetPlaylistByIdAsync(Guid id)
    {
        try
        {
            return await primaryService.GetPlaylistByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Primary service failed for GetPlaylistByIdAsync({PlaylistId}), falling back to fake data", id);
            return await fallbackService.GetPlaylistByIdAsync(id);
        }
    }
}
