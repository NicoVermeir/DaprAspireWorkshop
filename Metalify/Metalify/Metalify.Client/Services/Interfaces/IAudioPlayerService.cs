using Metalify.Client.Models;

namespace Metalify.Client.Services.Interfaces;

public interface IAudioPlayerService
{
    event EventHandler<Song?>? CurrentSongChanged;
    event EventHandler<bool>? PlayStateChanged;
    event EventHandler<TimeSpan>? PositionChanged;
    event EventHandler<TimeSpan>? DurationChanged;

    Song? CurrentSong { get; }
    bool IsPlaying { get; }
    TimeSpan Position { get; }
    TimeSpan Duration { get; }
    IEnumerable<Song> Queue { get; }

    Task PlayAsync(Song song);
    Task PlayAsync(IEnumerable<Song> songs, int startIndex = 0);
    Task PauseAsync();
    Task ResumeAsync();
    Task StopAsync();
    Task NextAsync();
    Task PreviousAsync();
    Task SeekAsync(TimeSpan position);
    Task SetVolumeAsync(double volume);
}
