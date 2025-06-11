using Metalify.Client.Models;
using Metalify.Services.Interfaces;

namespace Metalify.Services;

public class SearchService(IMusicDataService musicDataService, ILogger<SearchService> logger)
    : ISearchService
{
    public async Task<SearchResults> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new SearchResults();        logger.LogInformation("Searching for: {Query}", query);

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
        var artists = await musicDataService.GetArtistsAsync();
        return artists.Where(artist => 
            artist.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            artist.Genres.Any(g => g.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
            artist.Country.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Album>> SearchAlbumsAsync(string query)
    {
        var albums = await musicDataService.GetAlbumsAsync();
        return albums.Where(album => 
            album.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            album.ArtistName.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Song>> SearchSongsAsync(string query)
    {
        var songs = await musicDataService.GetSongsAsync();
        return songs.Where(song => 
            song.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            song.ArtistName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            song.AlbumTitle.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Playlist>> SearchPlaylistsAsync(string query)
    {
        var playlists = await musicDataService.GetPlaylistsAsync();
        return playlists.Where(playlist => 
            playlist.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            playlist.Description.Contains(query, StringComparison.OrdinalIgnoreCase));
    }
}
