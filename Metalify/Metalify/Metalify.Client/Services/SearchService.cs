using Metalify.Client.Models;
using Metalify.Client.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Metalify.Client.Services;

public class SearchService : ISearchService
{
    private readonly IMusicDataService _musicDataService;
    private readonly ILogger<SearchService> _logger;

    public SearchService(IMusicDataService musicDataService, ILogger<SearchService> logger)
    {
        _musicDataService = musicDataService;
        _logger = logger;
    }

    public async Task<SearchResults> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new SearchResults();        _logger.LogInformation("Searching for: {Query}", query);

        var artists = await SearchArtistsAsync(query);
        var albums = await SearchAlbumsAsync(query);
        var songs = await SearchSongsAsync(query);
        var playlists = await SearchPlaylistsAsync(query);

        return new SearchResults
        {
            Artists = artists,
            Albums = albums,
            Songs = songs,
            Playlists = playlists
        };
    }

    public async Task<IEnumerable<Artist>> SearchArtistsAsync(string query)
    {
        var artists = await _musicDataService.GetArtistsAsync();
        return artists.Where(a => 
            a.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            a.Genres.Any(g => g.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
            a.Country.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Album>> SearchAlbumsAsync(string query)
    {
        var albums = await _musicDataService.GetAlbumsAsync();
        return albums.Where(a => 
            a.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            a.ArtistName.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Song>> SearchSongsAsync(string query)
    {
        var songs = await _musicDataService.GetSongsAsync();
        return songs.Where(s => 
            s.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            s.ArtistName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            s.AlbumTitle.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Playlist>> SearchPlaylistsAsync(string query)
    {
        var playlists = await _musicDataService.GetPlaylistsAsync();
        return playlists.Where(p => 
            p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            p.Description.Contains(query, StringComparison.OrdinalIgnoreCase));
    }
}
