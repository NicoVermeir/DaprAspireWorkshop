using Metalify.Bandcenter.Api.DTOs;

namespace Metalify.Bandcenter.Api.Services;

public interface ISongService
{
    Task<IEnumerable<SongSummaryDto>> GetAllSongsAsync();    Task<SongDto?> GetSongByIdAsync(Guid id);
    Task<IEnumerable<SongSummaryDto>> GetSongsByAlbumIdAsync(Guid albumId);
    Task<IEnumerable<SongSummaryDto>> GetSongsByBandIdAsync(Guid bandId);
    Task<IEnumerable<SongSummaryDto>> SearchSongsByTitleAsync(string searchTerm);
    Task<IEnumerable<SongSummaryDto>> GetSongsByDurationRangeAsync(TimeSpan minDuration, TimeSpan maxDuration);
    Task<SongDto> CreateSongAsync(CreateSongDto createSongDto);
    Task<SongDto?> UpdateSongAsync(Guid id, UpdateSongDto updateSongDto);
    Task<bool> DeleteSongAsync(Guid id);
}
