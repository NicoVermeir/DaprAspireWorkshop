using Metalify.Bandcenter.Api.DTOs;

namespace Metalify.Bandcenter.Api.Services;

public interface IAlbumService
{
    Task<IEnumerable<AlbumSummaryDto>> GetAllAlbumsAsync();    Task<AlbumDto?> GetAlbumByIdAsync(Guid id);
    Task<AlbumDto?> GetAlbumWithSongsAsync(Guid id);
    Task<IEnumerable<AlbumSummaryDto>> GetAlbumsByBandIdAsync(Guid bandId);
    Task<IEnumerable<AlbumSummaryDto>> SearchAlbumsByTitleAsync(string searchTerm);
    Task<IEnumerable<AlbumSummaryDto>> GetAlbumsByAlbumTypeAsync(string albumType);
    Task<IEnumerable<AlbumSummaryDto>> GetAlbumsByYearAsync(int year);
    Task<IEnumerable<AlbumSummaryDto>> GetAlbumsByYearRangeAsync(int startYear, int endYear);
    Task<AlbumDto> CreateAlbumAsync(CreateAlbumDto createAlbumDto);
    Task<AlbumDto?> UpdateAlbumAsync(Guid id, UpdateAlbumDto updateAlbumDto);
    Task<bool> DeleteAlbumAsync(Guid id);
}
