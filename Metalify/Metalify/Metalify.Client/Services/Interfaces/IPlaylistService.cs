using Metalify.Client.Models;

namespace Metalify.Client.Services.Interfaces;

public interface IPlaylistService
{
    Task<IEnumerable<Playlist>> GetUserPlaylistsAsync();
    Task<Playlist> CreatePlaylistAsync(string name, string description = "");
    Task<bool> AddSongToPlaylistAsync(Guid playlistId, Guid songId);
    Task<bool> RemoveSongFromPlaylistAsync(Guid playlistId, Guid songId);
    Task<bool> DeletePlaylistAsync(Guid playlistId);
    Task<bool> UpdatePlaylistAsync(Playlist playlist);
}
