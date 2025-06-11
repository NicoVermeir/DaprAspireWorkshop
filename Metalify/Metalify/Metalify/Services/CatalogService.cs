using Metalify.Client.Models;
using Metalify.Services.Interfaces;
using System.Text.Json;

namespace Metalify.Services;

/// <summary>
/// HTTP-based music data service that calls the Metalify.Api with resilience patterns
/// </summary>
public class CatalogService(HttpClient httpClient, ILogger<CatalogService> logger) : ICatalogService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<IEnumerable<Artist>> GetArtistsAsync()
    {
        try
        {
            logger.LogInformation("Fetching all artists from API");
            
            var artistDtos = await httpClient.GetFromJsonAsync<List<ArtistSummaryDto>>(
                "api/artists", _jsonOptions);

            if (artistDtos == null)
            {
                logger.LogWarning("API returned null for artists");
                return [];
            }

            var artists = artistDtos.Select(MapToArtist).ToList();
            logger.LogInformation("Successfully fetched {Count} artists", artists.Count);
            
            return artists;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while fetching artists");
            throw;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "JSON deserialization error while fetching artists");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while fetching artists");
            throw;
        }
    }

    public async Task<IEnumerable<Album>> GetAlbumsAsync()
    {
        try
        {
            logger.LogInformation("Fetching all albums from API");
            
            var albumDtos = await httpClient.GetFromJsonAsync<List<AlbumSummaryDto>>(
                "api/albums", _jsonOptions);

            if (albumDtos == null)
            {
                logger.LogWarning("API returned null for albums");
                return [];
            }

            var albums = albumDtos.Select(MapToAlbum).ToList();
            logger.LogInformation("Successfully fetched {Count} albums", albums.Count);
            
            return albums;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while fetching albums");
            throw;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "JSON deserialization error while fetching albums");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while fetching albums");
            throw;
        }
    }

    public async Task<IEnumerable<Song>> GetSongsAsync()
    {
        try
        {
            logger.LogInformation("Fetching all songs from API");
            
            var songDtos = await httpClient.GetFromJsonAsync<List<SongDto>>(
                "api/songs", _jsonOptions);

            if (songDtos == null)
            {
                logger.LogWarning("API returned null for songs");
                return [];
            }

            var songs = songDtos.Select(MapToSong).ToList();
            logger.LogInformation("Successfully fetched {Count} songs", songs.Count);
            
            return songs;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while fetching songs");
            throw;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "JSON deserialization error while fetching songs");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while fetching songs");
            throw;
        }
    }

    public async Task<IEnumerable<Playlist>> GetPlaylistsAsync()
    {
        // Note: The API doesn't have playlists yet, so return empty for now
        logger.LogInformation("Playlists not implemented in API yet, returning empty list");
        await Task.Delay(1); // Maintain async signature
        return [];
    }

    public async Task<Artist?> GetArtistByIdAsync(Guid id)
    {
        try
        {
            logger.LogInformation("Fetching artist {ArtistId} from API", id);
            
            var artistDto = await httpClient.GetFromJsonAsync<ArtistDto>(
                $"api/artists/{id}", _jsonOptions);

            if (artistDto == null)
            {
                logger.LogWarning("Artist {ArtistId} not found", id);
                return null;
            }

            var artist = MapToArtistWithAlbums(artistDto);
            logger.LogInformation("Successfully fetched artist {ArtistName}", artist.Name);
            
            return artist;
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("404"))
        {
            logger.LogWarning("Artist {ArtistId} not found (404)", id);
            return null;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while fetching artist {ArtistId}", id);
            throw;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "JSON deserialization error while fetching artist {ArtistId}", id);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while fetching artist {ArtistId}", id);
            throw;
        }
    }

    public async Task<Album?> GetAlbumByIdAsync(Guid id)
    {
        try
        {
            logger.LogInformation("Fetching album {AlbumId} from API", id);
            
            var albumDto = await httpClient.GetFromJsonAsync<AlbumDto>(
                $"api/albums/{id}", _jsonOptions);

            if (albumDto == null)
            {
                logger.LogWarning("Album {AlbumId} not found", id);
                return null;
            }

            var album = MapToAlbumWithSongs(albumDto);
            logger.LogInformation("Successfully fetched album {AlbumTitle}", album.Title);
            
            return album;
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("404"))
        {
            logger.LogWarning("Album {AlbumId} not found (404)", id);
            return null;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while fetching album {AlbumId}", id);
            throw;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "JSON deserialization error while fetching album {AlbumId}", id);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while fetching album {AlbumId}", id);
            throw;
        }
    }

    public async Task<Song?> GetSongByIdAsync(Guid id)
    {
        try
        {
            logger.LogInformation("Fetching song {SongId} from API", id);
            
            var songDto = await httpClient.GetFromJsonAsync<SongDto>(
                $"api/songs/{id}", _jsonOptions);

            if (songDto == null)
            {
                logger.LogWarning("Song {SongId} not found", id);
                return null;
            }

            var song = MapToSong(songDto);
            logger.LogInformation("Successfully fetched song {SongTitle}", song.Title);
            
            return song;
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("404"))
        {
            logger.LogWarning("Song {SongId} not found (404)", id);
            return null;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error occurred while fetching song {SongId}", id);
            throw;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "JSON deserialization error while fetching song {SongId}", id);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while fetching song {SongId}", id);
            throw;
        }
    }

    public async Task<Playlist?> GetPlaylistByIdAsync(Guid id)
    {
        // Note: The API doesn't have playlists yet, so return null for now
        logger.LogInformation("Playlists not implemented in API yet, returning null for {PlaylistId}", id);
        await Task.Delay(1); // Maintain async signature
        return null;
    }

    // Mapping methods to convert DTOs to domain models
    private static Artist MapToArtist(ArtistSummaryDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        ImageUrl = dto.ImageUrl,
        Country = dto.Country,
        FormedYear = dto.FormedYear,
        Genres = dto.Genres,
        Bio = string.Empty, // Summary DTO doesn't include bio
        Albums = new List<Album>()
    };

    private static Artist MapToArtistWithAlbums(ArtistDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        ImageUrl = dto.ImageUrl,
        Bio = dto.Bio,
        Country = dto.Country,
        FormedYear = dto.FormedYear,
        Genres = dto.Genres,
        Albums = dto.Albums?.Select(MapToAlbum).ToList() ?? new List<Album>()
    };

    private static Album MapToAlbum(AlbumSummaryDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        CoverImageUrl = dto.CoverImageUrl,
        ReleaseYear = dto.ReleaseYear,
        ArtistId = dto.ArtistId,
        ArtistName = dto.ArtistName,
        Songs = new List<Song>()
    };

    private static Album MapToAlbumWithSongs(AlbumDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        CoverImageUrl = dto.CoverImageUrl,
        ReleaseYear = dto.ReleaseYear,
        ArtistId = dto.ArtistId,
        ArtistName = dto.ArtistName,
        Songs = dto.Songs?.Select(MapToSong).ToList() ?? new List<Song>()
    };

    private static Song MapToSong(SongDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        TrackNumber = dto.TrackNumber,
        Duration = dto.Duration,
        AlbumId = dto.AlbumId,
        AlbumTitle = dto.AlbumTitle,
        ArtistId = dto.ArtistId,
        ArtistName = dto.ArtistName,
        AudioUrl = dto.AudioUrl
    };
}
