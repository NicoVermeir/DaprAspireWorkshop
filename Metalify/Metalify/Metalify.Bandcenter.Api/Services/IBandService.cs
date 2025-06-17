using Metalify.Bandcenter.Api.DTOs;

namespace Metalify.Bandcenter.Api.Services;

public interface IBandService
{
    Task<IEnumerable<BandSummaryDto>> GetAllBandsAsync();    Task<BandDto?> GetBandByIdAsync(Guid id);
    Task<BandDto?> GetBandWithAlbumsAsync(Guid id);
    Task<IEnumerable<BandSummaryDto>> SearchBandsByNameAsync(string searchTerm);
    Task<IEnumerable<BandSummaryDto>> GetBandsByGenreAsync(string genre);
    Task<IEnumerable<BandSummaryDto>> GetBandsByCountryAsync(string country);
    Task<BandDto> CreateBandAsync(CreateBandDto createBandDto);
    Task<BandDto?> UpdateBandAsync(Guid id, UpdateBandDto updateBandDto);
    Task<bool> DeleteBandAsync(Guid id);
}
