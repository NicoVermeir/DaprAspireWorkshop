using Metalify.BandCenter.Models;

namespace Metalify.BandCenter.Services;

public interface IBandSessionService
{
    Band? CurrentBand { get; }
    event EventHandler<Band?> BandChanged;
    Task SetCurrentBandAsync(Band? band);
    Task ClearCurrentBandAsync();
    Task<Band?> GetCurrentBandAsync();
    bool IsLoggedIn => CurrentBand != null;
}
