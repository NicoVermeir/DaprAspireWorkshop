using Metalify.Client.Models;

namespace Metalify.Services.Interfaces;

public interface IMusicDataService
{
    Task<IEnumerable<Artist>> GetArtistsAsync();
    Task<IEnumerable<Album>> GetAlbumsAsync();
    Task<IEnumerable<Song>> GetSongsAsync();
    Task<IEnumerable<Playlist>> GetPlaylistsAsync();
    Task<Artist?> GetArtistByIdAsync(Guid id);
    Task<Album?> GetAlbumByIdAsync(Guid id);
    Task<Song?> GetSongByIdAsync(Guid id);
    Task<Playlist?> GetPlaylistByIdAsync(Guid id);
}
