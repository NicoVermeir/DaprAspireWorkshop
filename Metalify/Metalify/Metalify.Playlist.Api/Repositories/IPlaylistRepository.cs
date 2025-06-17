using Metalify.Playlist.Api.Models;

namespace Metalify.Playlist.Api.Repositories;

/// <summary>
/// Repository interface for playlist operations
/// </summary>
public interface IPlaylistRepository
{
    Task<IEnumerable<Models.Playlist>> GetAllAsync();
    Task<Models.Playlist?> GetByIdAsync(Guid id);
    Task<IEnumerable<Models.Playlist>> GetByCreatedByAsync(string createdBy);
    Task<IEnumerable<Models.Playlist>> GetPublicPlaylistsAsync();
    Task<IEnumerable<Models.Playlist>> SearchByNameAsync(string searchTerm);
    Task<Models.Playlist> CreateAsync(Models.Playlist playlist);
    Task<Models.Playlist?> UpdateAsync(Guid id, Models.Playlist playlist);
    Task<bool> DeleteAsync(Guid id);
    Task<PlaylistItem> AddSongToPlaylistAsync(Guid playlistId, Guid songId, int? position = null, 
        string songTitle = "", string artistName = "", string albumTitle = "", TimeSpan duration = default);
    Task<bool> RemoveSongFromPlaylistAsync(Guid playlistId, Guid songId);
    Task<bool> UpdatePlaylistItemPositionAsync(Guid itemId, int newPosition);
    Task<bool> ReorderPlaylistAsync(Guid playlistId, List<(Guid itemId, int position)> newOrder);
}
