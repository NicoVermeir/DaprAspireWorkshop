using Metalify.Client.Models;
using Metalify.Services.Interfaces;

namespace Metalify.Services;

public class PlaylistService(IMusicDataService musicDataService, ILogger<PlaylistService> logger)
    : IPlaylistService
{
    private readonly List<Playlist> _playlists = [];

    public async Task<IEnumerable<Playlist>> GetUserPlaylistsAsync()
    {
        await Task.Delay(50);
        return _playlists;
    }

    public async Task<Playlist> CreatePlaylistAsync(string name, string description = "")
    {
        await Task.Delay(100);
        
        var playlist = new Playlist
        {        Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            CoverImageUrl = "https://www.metal-archives.com/images/5/2/7/6/5276_logo.jpg",
            Songs = new List<Song>()
        };

        _playlists.Add(playlist);
        logger.LogInformation("Created playlist: {PlaylistName}", name);
        
        return playlist;
    }

    public async Task<bool> AddSongToPlaylistAsync(Guid playlistId, Guid songId)
    {
        await Task.Delay(50);
        
        var playlist = _playlists.FirstOrDefault(p => p.Id == playlistId);
        if (playlist == null)
        {
            logger.LogWarning("Playlist not found: {PlaylistId}", playlistId);
            return false;
        }

        var song = await musicDataService.GetSongByIdAsync(songId);
        if (song == null)
        {
            logger.LogWarning("Song not found: {SongId}", songId);
            return false;
        }

        if (!playlist.Songs.Any(s => s.Id == songId))
        {
            playlist.Songs.Add(song);
            playlist.UpdatedAt = DateTime.Now;
            logger.LogInformation("Added song {SongTitle} to playlist {PlaylistName}", song.Title, playlist.Name);
        }

        return true;
    }

    public async Task<bool> RemoveSongFromPlaylistAsync(Guid playlistId, Guid songId)
    {
        await Task.Delay(50);
        
        var playlist = _playlists.FirstOrDefault(p => p.Id == playlistId);
        if (playlist == null) return false;

        var songToRemove = playlist.Songs.FirstOrDefault(s => s.Id == songId);
        if (songToRemove != null)
        {
            playlist.Songs.Remove(songToRemove);
            playlist.UpdatedAt = DateTime.Now;
            logger.LogInformation("Removed song {SongTitle} from playlist {PlaylistName}", songToRemove.Title, playlist.Name);
            return true;
        }

        return false;
    }

    public async Task<bool> DeletePlaylistAsync(Guid playlistId)
    {
        await Task.Delay(50);
        
        var playlist = _playlists.FirstOrDefault(p => p.Id == playlistId);
        if (playlist != null)
        {
            _playlists.Remove(playlist);
            logger.LogInformation("Deleted playlist: {PlaylistName}", playlist.Name);
            return true;
        }

        return false;
    }

    public async Task<bool> UpdatePlaylistAsync(Playlist playlist)
    {
        await Task.Delay(50);
        
        var existingPlaylist = _playlists.FirstOrDefault(p => p.Id == playlist.Id);
        if (existingPlaylist != null)
        {
            existingPlaylist.Name = playlist.Name;
            existingPlaylist.Description = playlist.Description;
            existingPlaylist.UpdatedAt = DateTime.Now;
            logger.LogInformation("Updated playlist: {PlaylistName}", playlist.Name);
            return true;
        }

        return false;
    }
}
