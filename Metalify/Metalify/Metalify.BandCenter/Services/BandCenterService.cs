using System.Text.Json;
using Dapr.Client;
using Metalify.BandCenter.Models.DTOs;

namespace Metalify.BandCenter.Services;

public class BandCenterService(ILogger<BandCenterService> logger) : IBandCenterService
{
    private readonly HttpClient _httpClient = DaprClient.CreateInvokeHttpClient("metalify-bandcenter-api");

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    // Band operations
    public async Task<List<BandSummaryDto>> GetAllBandsAsync()
    {
        try
        {
            logger.LogInformation("Fetching all bands from Bandcenter API");
            
            var response = await _httpClient.GetFromJsonAsync<List<BandSummaryDto>>(
                "api/bands", _jsonOptions);
                
            return response ?? new List<BandSummaryDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching bands");
            return new List<BandSummaryDto>();
        }
    }

    public async Task<BandDto?> GetBandByIdAsync(Guid bandId)
    {
        try
        {
            logger.LogInformation("Fetching band {BandId} from Bandcenter API", bandId);
            
            return await _httpClient.GetFromJsonAsync<BandDto>(
                $"api/bands/{bandId}", _jsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching band {BandId}", bandId);
            return null;
        }
    }

    public async Task<BandDto> CreateBandAsync(CreateBandDto createBand)
    {
        try
        {
            logger.LogInformation("Creating new band: {BandName}", createBand.Name);
            
            var response = await _httpClient.PostAsJsonAsync("api/bands", createBand, _jsonOptions);
            response.EnsureSuccessStatusCode();
            
            var createdBand = await response.Content.ReadFromJsonAsync<BandDto>(_jsonOptions);
            return createdBand ?? throw new InvalidOperationException("Failed to create band");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating band: {BandName}", createBand.Name);
            throw;
        }
    }

    public async Task<BandDto?> UpdateBandAsync(Guid bandId, UpdateBandDto updateBand)
    {
        try
        {
            logger.LogInformation("Updating band {BandId}", bandId);
            
            var response = await _httpClient.PutAsJsonAsync($"api/bands/{bandId}", updateBand, _jsonOptions);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<BandDto>(_jsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating band {BandId}", bandId);
            return null;
        }
    }

    public async Task<bool> DeleteBandAsync(Guid bandId)
    {
        try
        {
            logger.LogInformation("Deleting band {BandId}", bandId);
            
            var response = await _httpClient.DeleteAsync($"api/bands/{bandId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting band {BandId}", bandId);
            return false;
        }
    }

    // Album operations
    public async Task<List<AlbumSummaryDto>> GetBandAlbumsAsync(Guid bandId)
    {
        try
        {
            logger.LogInformation("Fetching albums for band {BandId}", bandId);
            
            var response = await _httpClient.GetFromJsonAsync<List<AlbumSummaryDto>>(
                $"api/albums/band/{bandId}", _jsonOptions);
            
            return response ?? new List<AlbumSummaryDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching albums for band {BandId}", bandId);
            return new List<AlbumSummaryDto>();
        }
    }

    public async Task<AlbumDto?> GetAlbumByIdAsync(Guid albumId)
    {
        try
        {
            logger.LogInformation("Fetching album {AlbumId}", albumId);
            
            return await _httpClient.GetFromJsonAsync<AlbumDto>(
                $"api/albums/{albumId}", _jsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching album {AlbumId}", albumId);
            return null;
        }
    }

    public async Task<AlbumDto> CreateAlbumAsync(CreateAlbumDto createAlbum)
    {
        try
        {
            logger.LogInformation("Creating new album: {AlbumTitle} for band {BandId}", 
                createAlbum.Title, createAlbum.BandId);
            
            var response = await _httpClient.PostAsJsonAsync("api/albums", createAlbum, _jsonOptions);
            response.EnsureSuccessStatusCode();
            
            var createdAlbum = await response.Content.ReadFromJsonAsync<AlbumDto>(_jsonOptions);
            return createdAlbum ?? throw new InvalidOperationException("Failed to create album");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating album: {AlbumTitle}", createAlbum.Title);
            throw;
        }
    }

    public async Task<AlbumDto?> UpdateAlbumAsync(Guid albumId, UpdateAlbumDto updateAlbum)
    {
        try
        {
            logger.LogInformation("Updating album {AlbumId}", albumId);
            
            var response = await _httpClient.PutAsJsonAsync($"api/albums/{albumId}", updateAlbum, _jsonOptions);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<AlbumDto>(_jsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating album {AlbumId}", albumId);
            return null;
        }
    }

    public async Task<bool> DeleteAlbumAsync(Guid albumId)
    {
        try
        {
            logger.LogInformation("Deleting album {AlbumId}", albumId);
            
            var response = await _httpClient.DeleteAsync($"api/albums/{albumId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting album {AlbumId}", albumId);
            return false;
        }
    }

    // Song operations
    public async Task<AlbumDto> GetAlbumSongsAsync(Guid albumId)
    {
        try
        {
            logger.LogInformation("Fetching songs for album {AlbumId}", albumId);
            
            var response = await _httpClient.GetFromJsonAsync<AlbumDto>(
                $"api/albums/{albumId}/songs", _jsonOptions);
            return response ?? new AlbumDto();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching songs for album {AlbumId}", albumId);
            return new AlbumDto();
        }
    }

    public async Task<SongDto?> GetSongByIdAsync(Guid songId)
    {
        try
        {
            logger.LogInformation("Fetching song {SongId}", songId);
            
            return await _httpClient.GetFromJsonAsync<SongDto>(
                $"api/songs/{songId}", _jsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching song {SongId}", songId);
            return null;
        }
    }

    public async Task<SongDto> CreateSongAsync(CreateSongDto createSong)
    {
        try
        {
            logger.LogInformation("Creating new song: {SongTitle} for album {AlbumId}", 
                createSong.Title, createSong.AlbumId);
            
            var response = await _httpClient.PostAsJsonAsync("api/songs", createSong, _jsonOptions);
            response.EnsureSuccessStatusCode();
            
            var createdSong = await response.Content.ReadFromJsonAsync<SongDto>(_jsonOptions);
            return createdSong ?? throw new InvalidOperationException("Failed to create song");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating song: {SongTitle}", createSong.Title);
            throw;
        }
    }

    public async Task<SongDto?> UpdateSongAsync(Guid songId, UpdateSongDto updateSong)
    {
        try
        {
            logger.LogInformation("Updating song {SongId}", songId);
            
            var response = await _httpClient.PutAsJsonAsync($"api/songs/{songId}", updateSong, _jsonOptions);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<SongDto>(_jsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating song {SongId}", songId);
            return null;
        }
    }

    public async Task<bool> DeleteSongAsync(Guid songId)
    {
        try
        {
            logger.LogInformation("Deleting song {SongId}", songId);
            
            var response = await _httpClient.DeleteAsync($"api/songs/{songId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting song {SongId}", songId);
            return false;
        }
    }
}
