using Microsoft.EntityFrameworkCore;
using Metalify.Playlist.Api.Data;
using Metalify.Playlist.Api.Models;

namespace Metalify.Playlist.Api.Repositories;

/// <summary>
/// Entity Framework implementation of the Playlist repository
/// </summary>
public class PlaylistRepository : IPlaylistRepository
{
    private readonly PlaylistDbContext _context;
    private readonly ILogger<PlaylistRepository> _logger;

    public PlaylistRepository(PlaylistDbContext context, ILogger<PlaylistRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Models.Playlist>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all playlists");
            return await _context.Playlists
                .Include(p => p.PlaylistItems)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all playlists");
            throw;
        }
    }

    public async Task<Models.Playlist?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Retrieving playlist with ID: {PlaylistId}", id);
            return await _context.Playlists
                .Include(p => p.PlaylistItems.OrderBy(pi => pi.Position))
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving playlist with ID: {PlaylistId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Models.Playlist>> GetByCreatedByAsync(string createdBy)
    {
        try
        {
            _logger.LogInformation("Retrieving playlists created by: {CreatedBy}", createdBy);
            return await _context.Playlists
                .Include(p => p.PlaylistItems)
                .Where(p => p.CreatedBy == createdBy)
                .OrderByDescending(p => p.UpdatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving playlists for user: {CreatedBy}", createdBy);
            throw;
        }
    }

    public async Task<IEnumerable<Models.Playlist>> GetPublicPlaylistsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving public playlists");
            return await _context.Playlists
                .Include(p => p.PlaylistItems)
                .Where(p => p.IsPublic)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving public playlists");
            throw;
        }
    }

    public async Task<IEnumerable<Models.Playlist>> SearchByNameAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation("Searching playlists with term: {SearchTerm}", searchTerm);
            return await _context.Playlists
                .Include(p => p.PlaylistItems)
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching playlists with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<Models.Playlist> CreateAsync(Models.Playlist playlist)
    {
        try
        {
            _logger.LogInformation("Creating playlist: {PlaylistName}", playlist.Name);
            playlist.Id = Guid.NewGuid();
            playlist.CreatedAt = DateTime.UtcNow;
            playlist.UpdatedAt = DateTime.UtcNow;

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully created playlist: {PlaylistName} with ID: {PlaylistId}", 
                playlist.Name, playlist.Id);
            return playlist;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating playlist: {PlaylistName}", playlist.Name);
            throw;
        }
    }

    public async Task<Models.Playlist?> UpdateAsync(Guid id, Models.Playlist playlist)
    {
        try
        {
            _logger.LogInformation("Updating playlist with ID: {PlaylistId}", id);
            var existingPlaylist = await _context.Playlists.FindAsync(id);
            
            if (existingPlaylist == null)
            {
                _logger.LogWarning("Playlist not found for update: {PlaylistId}", id);
                return null;
            }

            existingPlaylist.Name = playlist.Name;
            existingPlaylist.Description = playlist.Description;
            existingPlaylist.CoverImageUrl = playlist.CoverImageUrl;
            existingPlaylist.IsPublic = playlist.IsPublic;
            existingPlaylist.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully updated playlist: {PlaylistId}", id);
            return existingPlaylist;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating playlist: {PlaylistId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting playlist with ID: {PlaylistId}", id);
            var playlist = await _context.Playlists.FindAsync(id);
            
            if (playlist == null)
            {
                _logger.LogWarning("Playlist not found for deletion: {PlaylistId}", id);
                return false;
            }

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted playlist: {PlaylistId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting playlist: {PlaylistId}", id);
            throw;
        }
    }    public async Task<PlaylistItem> AddSongToPlaylistAsync(Guid playlistId, Guid songId, int? position = null, 
        string songTitle = "", string artistName = "", string albumTitle = "", string albumCoverImageUrl = "", TimeSpan duration = default)
    {
        try
        {
            _logger.LogInformation("Adding song {SongId} ({SongTitle}) to playlist {PlaylistId}", songId, songTitle, playlistId);
            
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistItems)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null)
            {
                throw new ArgumentException($"Playlist with ID {playlistId} not found");
            }

            // Check if song already exists in playlist
            if (playlist.PlaylistItems.Any(pi => pi.SongId == songId))
            {
                throw new InvalidOperationException($"Song {songId} already exists in playlist {playlistId}");
            }

            // Calculate position
            var itemPosition = position ?? (playlist.PlaylistItems.Any() ? 
                playlist.PlaylistItems.Max(pi => pi.Position) + 1 : 1);

            // If inserting at specific position, shift other items
            if (position.HasValue)
            {
                var itemsToShift = playlist.PlaylistItems
                    .Where(pi => pi.Position >= position.Value)
                    .ToList();

                foreach (var item in itemsToShift)
                {
                    item.Position++;
                }
            }            var playlistItem = new PlaylistItem
            {
                Id = Guid.NewGuid(),
                PlaylistId = playlistId,
                SongId = songId,
                SongTitle = songTitle,
                ArtistName = artistName,
                AlbumTitle = albumTitle,
                AlbumCoverImageUrl = albumCoverImageUrl,
                Duration = duration,
                Position = itemPosition,
                AddedAt = DateTime.UtcNow
            };

            _context.PlaylistItems.Add(playlistItem);
            playlist.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully added song {SongId} ({SongTitle}) to playlist {PlaylistId} at position {Position}", 
                songId, songTitle, playlistId, itemPosition);
            return playlistItem;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding song {SongId} to playlist {PlaylistId}", songId, playlistId);
            throw;
        }
    }

    public async Task<bool> RemoveSongFromPlaylistAsync(Guid playlistId, Guid songId)
    {
        try
        {
            _logger.LogInformation("Removing song {SongId} from playlist {PlaylistId}", songId, playlistId);
            
            var playlistItem = await _context.PlaylistItems
                .FirstOrDefaultAsync(pi => pi.PlaylistId == playlistId && pi.SongId == songId);

            if (playlistItem == null)
            {
                _logger.LogWarning("Song {SongId} not found in playlist {PlaylistId}", songId, playlistId);
                return false;
            }

            var removedPosition = playlistItem.Position;
            _context.PlaylistItems.Remove(playlistItem);

            // Adjust positions of subsequent items
            var itemsToAdjust = await _context.PlaylistItems
                .Where(pi => pi.PlaylistId == playlistId && pi.Position > removedPosition)
                .ToListAsync();

            foreach (var item in itemsToAdjust)
            {
                item.Position--;
            }

            // Update playlist timestamp
            var playlist = await _context.Playlists.FindAsync(playlistId);
            if (playlist != null)
            {
                playlist.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully removed song {SongId} from playlist {PlaylistId}", songId, playlistId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing song {SongId} from playlist {PlaylistId}", songId, playlistId);
            throw;
        }
    }

    public async Task<bool> UpdatePlaylistItemPositionAsync(Guid itemId, int newPosition)
    {
        try
        {
            _logger.LogInformation("Updating position of playlist item {ItemId} to {NewPosition}", itemId, newPosition);
            
            var item = await _context.PlaylistItems.FindAsync(itemId);
            if (item == null)
            {
                _logger.LogWarning("Playlist item not found: {ItemId}", itemId);
                return false;
            }

            var oldPosition = item.Position;
            item.Position = newPosition;

            // Adjust other items' positions
            var otherItems = await _context.PlaylistItems
                .Where(pi => pi.PlaylistId == item.PlaylistId && pi.Id != itemId)
                .ToListAsync();

            if (newPosition > oldPosition)
            {
                // Moving down - shift items up
                foreach (var otherItem in otherItems.Where(pi => pi.Position > oldPosition && pi.Position <= newPosition))
                {
                    otherItem.Position--;
                }
            }
            else if (newPosition < oldPosition)
            {
                // Moving up - shift items down
                foreach (var otherItem in otherItems.Where(pi => pi.Position >= newPosition && pi.Position < oldPosition))
                {
                    otherItem.Position++;
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully updated position of playlist item {ItemId}", itemId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating position of playlist item {ItemId}", itemId);
            throw;
        }
    }

    public async Task<bool> ReorderPlaylistAsync(Guid playlistId, List<(Guid itemId, int position)> newOrder)
    {
        try
        {
            _logger.LogInformation("Reordering playlist {PlaylistId}", playlistId);
            
            var items = await _context.PlaylistItems
                .Where(pi => pi.PlaylistId == playlistId)
                .ToListAsync();

            foreach (var (itemId, position) in newOrder)
            {
                var item = items.FirstOrDefault(pi => pi.Id == itemId);
                if (item != null)
                {
                    item.Position = position;
                }
            }

            // Update playlist timestamp
            var playlist = await _context.Playlists.FindAsync(playlistId);
            if (playlist != null)
            {
                playlist.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully reordered playlist {PlaylistId}", playlistId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reordering playlist {PlaylistId}", playlistId);
            throw;
        }
    }
}
