using Metalify.Playlist.Api.DTOs;
using System.Text.Json;

namespace Metalify.Playlist.Api.Services;

/// <summary>
/// Service interface for catalog API communication
/// </summary>
public interface ICatalogApiService
{
    Task<SongDto?> GetSongByIdAsync(Guid songId);
    Task<IEnumerable<SongDto>> GetSongsByIdsAsync(IEnumerable<Guid> songIds);
}

/// <summary>
/// HTTP client service for communicating with the main catalog API
/// </summary>
public class CatalogApiService : ICatalogApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CatalogApiService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public CatalogApiService(HttpClient httpClient, ILogger<CatalogApiService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<SongDto?> GetSongByIdAsync(Guid songId)
    {
        try
        {
            _logger.LogInformation("Fetching song {SongId} from catalog API", songId);
            
            var response = await _httpClient.GetAsync($"api/songs/{songId}");
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Song {SongId} not found in catalog", songId);
                return null;
            }

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var songDto = JsonSerializer.Deserialize<SongDto>(content, _jsonOptions);

            _logger.LogInformation("Successfully fetched song {SongTitle}", songDto?.Title ?? "Unknown");
            return songDto;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error occurred while fetching song {SongId}", songId);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error while fetching song {SongId}", songId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching song {SongId}", songId);
            throw;
        }
    }

    public async Task<IEnumerable<SongDto>> GetSongsByIdsAsync(IEnumerable<Guid> songIds)
    {
        var songs = new List<SongDto>();
        
        foreach (var songId in songIds)
        {
            var song = await GetSongByIdAsync(songId);
            if (song != null)
            {
                songs.Add(song);
            }
        }
        
        return songs;
    }
}
