using Metalify.Client.Models;
using Metalify.Services.Interfaces;
using System.Text.Json;

namespace Metalify.Services;

/// <summary>
/// HTTP-based playlist service that calls the Metalify.Playlist.Api
/// </summary>
public class PlaylistService(HttpClient httpClient, ICatalogService catalogService, ILogger<PlaylistService> logger) : IPlaylistService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<IEnumerable<Playlist>> GetUserPlaylistsAsync()
    {
        try
        {
            logger.LogInformation("Fetching user playlists from API");
            
            var playlistDtos = await httpClient.GetFromJsonAsync<List<PlaylistSummaryDto>>(
                "api/playlists", _jsonOptions);

            if (playlistDtos == null)
            {
                logger.LogWarning("API returned null for playlists");
                return [];
            }

            var playlists = playlistDtos.Select(MapToPlaylist).ToList();
            logger.LogInformation("Successfully fetched {Count} playlists", playlists.Count);
            
            return playlists;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while fetching playlists");
            return [];
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "JSON deserialization error while fetching playlists");
            return [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while fetching playlists");
            return [];
        }
    }

    public async Task<Playlist> CreatePlaylistAsync(string name, string description = "")
    {
        try
        {
            logger.LogInformation("Creating playlist: {PlaylistName}", name);
            
            var createDto = new CreatePlaylistDto
            {
                Name = name,
                Description = description,
                CreatedBy = "user", // TODO: Get from authentication context
                IsPublic = true,
                CoverImageUrl = "https://www.metal-archives.com/images/5/2/7/6/5276_logo.jpg"
            };

            var response = await httpClient.PostAsJsonAsync("api/playlists", createDto, _jsonOptions);
            response.EnsureSuccessStatusCode();

            var playlistDto = await response.Content.ReadFromJsonAsync<PlaylistDto>(_jsonOptions);
            
            if (playlistDto == null)
            {
                throw new InvalidOperationException("API returned null playlist after creation");
            }

            logger.LogInformation("Successfully created playlist: {PlaylistName} with ID: {PlaylistId}", 
                name, playlistDto.Id);
            
            return MapToPlaylistWithSongs(playlistDto);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while creating playlist: {PlaylistName}", name);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating playlist: {PlaylistName}", name);
            throw;
        }
    }    public async Task<bool> AddSongToPlaylistAsync(Guid playlistId, Guid songId)
    {
        try
        {
            logger.LogInformation("Adding song {SongId} to playlist {PlaylistId}", songId, playlistId);
            
            // Fetch song metadata from catalog service
            var song = await catalogService.GetSongByIdAsync(songId);
            if (song == null)
            {
                logger.LogWarning("Song {SongId} not found in catalog", songId);
                return false;
            }
              var addSongDto = new AddSongToPlaylistDto
            {
                SongId = songId,
                SongTitle = song.Title,
                ArtistName = song.ArtistName,
                AlbumTitle = song.AlbumTitle,
                AlbumCoverImageUrl = song.AlbumCoverImageUrl,
                Duration = song.Duration,
                Position = null // Add to end
            };

            var response = await httpClient.PostAsJsonAsync(
                $"api/playlists/{playlistId}/songs", addSongDto, _jsonOptions);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully added song {SongId} ({SongTitle}) to playlist {PlaylistId}", 
                    songId, song.Title, playlistId);
                return true;
            }
            else
            {
                logger.LogWarning("Failed to add song {SongId} to playlist {PlaylistId}. Status: {StatusCode}", 
                    songId, playlistId, response.StatusCode);
                return false;
            }
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while adding song {SongId} to playlist {PlaylistId}", 
                songId, playlistId);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding song {SongId} to playlist {PlaylistId}", songId, playlistId);
            return false;
        }
    }

    public async Task<bool> RemoveSongFromPlaylistAsync(Guid playlistId, Guid songId)
    {
        try
        {
            logger.LogInformation("Removing song {SongId} from playlist {PlaylistId}", songId, playlistId);

            var response = await httpClient.DeleteAsync($"api/playlists/{playlistId}/songs/{songId}");

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully removed song {SongId} from playlist {PlaylistId}", 
                    songId, playlistId);
                return true;
            }
            else
            {
                logger.LogWarning("Failed to remove song {SongId} from playlist {PlaylistId}. Status: {StatusCode}", 
                    songId, playlistId, response.StatusCode);
                return false;
            }
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while removing song {SongId} from playlist {PlaylistId}", 
                songId, playlistId);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing song {SongId} from playlist {PlaylistId}", songId, playlistId);
            return false;
        }
    }

    public async Task<bool> DeletePlaylistAsync(Guid playlistId)
    {
        try
        {
            logger.LogInformation("Deleting playlist {PlaylistId}", playlistId);

            var response = await httpClient.DeleteAsync($"api/playlists/{playlistId}");

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully deleted playlist {PlaylistId}", playlistId);
                return true;
            }
            else
            {
                logger.LogWarning("Failed to delete playlist {PlaylistId}. Status: {StatusCode}", 
                    playlistId, response.StatusCode);
                return false;
            }
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while deleting playlist {PlaylistId}", playlistId);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting playlist {PlaylistId}", playlistId);
            return false;
        }
    }

    public async Task<bool> UpdatePlaylistAsync(Playlist playlist)
    {
        try
        {
            logger.LogInformation("Updating playlist {PlaylistId}: {PlaylistName}", playlist.Id, playlist.Name);

            var updateDto = new UpdatePlaylistDto
            {
                Name = playlist.Name,
                Description = playlist.Description,
                CoverImageUrl = playlist.CoverImageUrl,
                IsPublic = playlist.IsPublic
            };

            var response = await httpClient.PutAsJsonAsync($"api/playlists/{playlist.Id}", updateDto, _jsonOptions);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully updated playlist {PlaylistId}: {PlaylistName}", 
                    playlist.Id, playlist.Name);
                return true;
            }
            else
            {
                logger.LogWarning("Failed to update playlist {PlaylistId}. Status: {StatusCode}", 
                    playlist.Id, response.StatusCode);
                return false;
            }
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while updating playlist {PlaylistId}", playlist.Id);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating playlist {PlaylistId}", playlist.Id);
            return false;
        }
    }    // Helper methods to map between DTOs and domain models
    private static Playlist MapToPlaylist(PlaylistSummaryDto dto)
    {
        return new Playlist
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            CoverImageUrl = dto.CoverImageUrl,
            IsPublic = dto.IsPublic,
            CreatedBy = dto.CreatedBy,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            SongCount = dto.SongCount,
            Songs = new List<Song>() // Summary doesn't include songs
        };
    }    private static Playlist MapToPlaylistWithSongs(PlaylistDto dto)
    {
        var songs = dto.Songs?.Select(MapToSong).ToList() ?? new List<Song>();
        return new Playlist
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            CoverImageUrl = dto.CoverImageUrl,
            IsPublic = dto.IsPublic,
            CreatedBy = dto.CreatedBy,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            SongCount = songs.Count, // Calculate from actual songs list
            Songs = songs
        };
    }    private static Song MapToSong(PlaylistItemDto dto)
    {
        return new Song
        {
            Id = dto.SongId,
            Title = dto.SongTitle,
            Duration = dto.Duration,
            AlbumTitle = dto.AlbumTitle,
            AlbumCoverImageUrl = dto.AlbumCoverImageUrl,
            ArtistName = dto.ArtistName,
            // Note: Some properties may not be available in playlist context
            TrackNumber = 0, // Not available in playlist context
            AudioUrl = "", // Not available in playlist context
            AlbumId = Guid.Empty, // Not available in playlist context
            ArtistId = Guid.Empty // Not available in playlist context
        };
    }
}
