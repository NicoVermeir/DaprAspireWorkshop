using Metalify.BandCenter.Models;
using Metalify.BandCenter.Models.DTOs;

namespace Metalify.BandCenter.Services;

public interface IBandCenterService
{
    // Band operations
    Task<List<BandSummaryDto>> GetAllBandsAsync();
    Task<BandDto?> GetBandByIdAsync(Guid bandId);
    Task<BandDto> CreateBandAsync(CreateBandDto createBand);
    Task<BandDto?> UpdateBandAsync(Guid bandId, UpdateBandDto updateBand);
    Task<bool> DeleteBandAsync(Guid bandId);

    // Album operations
    Task<List<AlbumSummaryDto>> GetBandAlbumsAsync(Guid bandId);
    Task<AlbumDto?> GetAlbumByIdAsync(Guid albumId);
    Task<AlbumDto> CreateAlbumAsync(CreateAlbumDto createAlbum);
    Task<AlbumDto?> UpdateAlbumAsync(Guid albumId, UpdateAlbumDto updateAlbum);
    Task<bool> DeleteAlbumAsync(Guid albumId);

    // Song operations
    Task<AlbumDto> GetAlbumSongsAsync(Guid albumId);
    Task<SongDto?> GetSongByIdAsync(Guid songId);
    Task<SongDto> CreateSongAsync(CreateSongDto createSong);
    Task<SongDto?> UpdateSongAsync(Guid songId, UpdateSongDto updateSong);
    Task<bool> DeleteSongAsync(Guid songId);
}
