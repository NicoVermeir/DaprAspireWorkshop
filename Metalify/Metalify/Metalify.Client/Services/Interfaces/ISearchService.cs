using Metalify.Client.Models;

namespace Metalify.Client.Services.Interfaces;

public interface ISearchService
{
    Task<SearchResults> SearchAsync(string query);
    Task<IEnumerable<Artist>> SearchArtistsAsync(string query);
    Task<IEnumerable<Album>> SearchAlbumsAsync(string query);
    Task<IEnumerable<Song>> SearchSongsAsync(string query);
    Task<IEnumerable<Playlist>> SearchPlaylistsAsync(string query);
}

public class SearchResults
{
    public IEnumerable<Artist> Artists { get; set; } = new List<Artist>();
    public IEnumerable<Album> Albums { get; set; } = new List<Album>();
    public IEnumerable<Song> Songs { get; set; } = new List<Song>();
    public IEnumerable<Playlist> Playlists { get; set; } = new List<Playlist>();
}
