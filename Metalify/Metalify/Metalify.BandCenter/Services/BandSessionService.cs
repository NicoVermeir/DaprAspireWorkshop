using Metalify.BandCenter.Models;

namespace Metalify.BandCenter.Services;

public class BandSessionService : IBandSessionService
{
    private Band? _currentBand;

    public Band? CurrentBand => _currentBand;

    public event EventHandler<Band?>? BandChanged;

    public Task SetCurrentBandAsync(Band? band)
    {
        _currentBand = band;
        BandChanged?.Invoke(this, band);
        return Task.CompletedTask;
    }

    public Task ClearCurrentBandAsync()
    {
        return SetCurrentBandAsync(null);
    }

    public Task<Band?> GetCurrentBandAsync()
    {
        return Task.FromResult(_currentBand);
    }

    public bool IsLoggedIn => CurrentBand != null;
}
