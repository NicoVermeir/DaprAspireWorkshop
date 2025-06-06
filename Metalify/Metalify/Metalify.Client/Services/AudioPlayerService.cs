using Metalify.Client.Models;
using Metalify.Client.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Timers;

namespace Metalify.Client.Services;

public class AudioPlayerService : IAudioPlayerService, IDisposable
{
    private readonly ILogger<AudioPlayerService> _logger;
    private readonly System.Timers.Timer _positionTimer;
    private readonly List<Song> _queue = new();
    private int _currentIndex = -1;
    private bool _isPlaying = false;
    private TimeSpan _position = TimeSpan.Zero;
    private TimeSpan _duration = TimeSpan.Zero;
    private double _volume = 1.0;

    public event EventHandler<Song?>? CurrentSongChanged;
    public event EventHandler<bool>? PlayStateChanged;
    public event EventHandler<TimeSpan>? PositionChanged;
    public event EventHandler<TimeSpan>? DurationChanged;

    public Song? CurrentSong => _currentIndex >= 0 && _currentIndex < _queue.Count ? _queue[_currentIndex] : null;
    public bool IsPlaying => _isPlaying;
    public TimeSpan Position => _position;
    public TimeSpan Duration => _duration;
    public IEnumerable<Song> Queue => _queue;

    public AudioPlayerService(ILogger<AudioPlayerService> logger)
    {
        _logger = logger;
        _positionTimer = new System.Timers.Timer(1000); // Update every second
        _positionTimer.Elapsed += OnPositionTimerElapsed;
    }

    public async Task PlayAsync(Song song)
    {
        await PlayAsync(new[] { song }, 0);
    }

    public async Task PlayAsync(IEnumerable<Song> songs, int startIndex = 0)
    {
        await Task.Delay(100); // Simulate loading time

        _queue.Clear();
        _queue.AddRange(songs);
        _currentIndex = startIndex;

        if (_currentIndex >= 0 && _currentIndex < _queue.Count)
        {
            var currentSong = _queue[_currentIndex];
            _duration = currentSong.Duration;
            _position = TimeSpan.Zero;
            _isPlaying = true;

            _positionTimer.Start();

            _logger.LogInformation("Now playing: {ArtistName} - {SongTitle}", currentSong.ArtistName, currentSong.Title);

            CurrentSongChanged?.Invoke(this, currentSong);
            PlayStateChanged?.Invoke(this, true);
            DurationChanged?.Invoke(this, _duration);
            PositionChanged?.Invoke(this, _position);
        }
    }

    public async Task PauseAsync()
    {
        await Task.Delay(50);
        
        if (_isPlaying)
        {
            _isPlaying = false;
            _positionTimer.Stop();
            _logger.LogInformation("Playback paused");
            PlayStateChanged?.Invoke(this, false);
        }
    }

    public async Task ResumeAsync()
    {
        await Task.Delay(50);
        
        if (!_isPlaying && CurrentSong != null)
        {
            _isPlaying = true;
            _positionTimer.Start();
            _logger.LogInformation("Playback resumed");
            PlayStateChanged?.Invoke(this, true);
        }
    }

    public async Task StopAsync()
    {
        await Task.Delay(50);
        
        _isPlaying = false;
        _positionTimer.Stop();
        _position = TimeSpan.Zero;
        _currentIndex = -1;
        _queue.Clear();

        _logger.LogInformation("Playback stopped");

        CurrentSongChanged?.Invoke(this, null);
        PlayStateChanged?.Invoke(this, false);
        PositionChanged?.Invoke(this, _position);
    }

    public async Task NextAsync()
    {
        if (_currentIndex < _queue.Count - 1)
        {
            _currentIndex++;
            await PlayCurrentSong();
        }
        else
        {
            await StopAsync();
        }
    }

    public async Task PreviousAsync()
    {
        if (_currentIndex > 0)
        {
            _currentIndex--;
            await PlayCurrentSong();
        }
        else if (_currentIndex == 0)
        {
            // Restart current song
            await SeekAsync(TimeSpan.Zero);
        }
    }

    public async Task SeekAsync(TimeSpan position)
    {
        await Task.Delay(50);
        
        if (CurrentSong != null && position <= _duration)
        {
            _position = position;
            _logger.LogDebug("Seeked to position: {Position}", position);
            PositionChanged?.Invoke(this, _position);
        }
    }

    public async Task SetVolumeAsync(double volume)
    {
        await Task.Delay(50);
        
        _volume = Math.Clamp(volume, 0.0, 1.0);
        _logger.LogDebug("Volume set to: {Volume}", _volume);
    }

    private async Task PlayCurrentSong()
    {
        if (_currentIndex >= 0 && _currentIndex < _queue.Count)
        {
            var currentSong = _queue[_currentIndex];
            _duration = currentSong.Duration;
            _position = TimeSpan.Zero;

            CurrentSongChanged?.Invoke(this, currentSong);
            DurationChanged?.Invoke(this, _duration);
            PositionChanged?.Invoke(this, _position);

            if (_isPlaying)
            {
                _positionTimer.Start();
            }

            _logger.LogInformation("Now playing: {ArtistName} - {SongTitle}", currentSong.ArtistName, currentSong.Title);
        }
    }

    private void OnPositionTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if (_isPlaying && CurrentSong != null)
        {
            _position = _position.Add(TimeSpan.FromSeconds(1));
            
            if (_position >= _duration)
            {
                // Song finished, play next
                Task.Run(async () => await NextAsync());
            }
            else
            {
                PositionChanged?.Invoke(this, _position);
            }
        }
    }

    public void Dispose()
    {
        _positionTimer?.Dispose();
    }
}
