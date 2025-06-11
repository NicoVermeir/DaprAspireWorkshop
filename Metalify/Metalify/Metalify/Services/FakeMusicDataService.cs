using Metalify.Client.Models;
using Metalify.Services.Interfaces;

namespace Metalify.Services;

public class FakeMusicDataService : IMusicDataService
{
    private readonly ILogger<FakeMusicDataService> _logger;
    private readonly List<Artist> _artists;
    private readonly List<Album> _albums;
    private readonly List<Song> _songs;
    private readonly List<Playlist> _playlists;

    public FakeMusicDataService(ILogger<FakeMusicDataService> logger)
    {
        _logger = logger;
        _artists = GenerateFakeArtists();
        _albums = GenerateFakeAlbums();
        _songs = GenerateFakeSongs();
        _playlists = GenerateFakePlaylists();
        
        _logger.LogInformation("Initialized fake music data service with {ArtistCount} artists, {AlbumCount} albums, {SongCount} songs", 
            _artists.Count, _albums.Count, _songs.Count);
    }

    public async Task<IEnumerable<Artist>> GetArtistsAsync()
    {
        await Task.Delay(100); // Simulate network delay
        return _artists;
    }

    public async Task<IEnumerable<Album>> GetAlbumsAsync()
    {
        await Task.Delay(100);
        return _albums;
    }

    public async Task<IEnumerable<Song>> GetSongsAsync()
    {
        await Task.Delay(100);
        return _songs;
    }

    public async Task<IEnumerable<Playlist>> GetPlaylistsAsync()
    {
        await Task.Delay(100);
        return _playlists;
    }

    public async Task<Artist?> GetArtistByIdAsync(Guid id)
    {
        await Task.Delay(50);
        return _artists.FirstOrDefault(a => a.Id == id);
    }

    public async Task<Album?> GetAlbumByIdAsync(Guid id)
    {
        await Task.Delay(50);
        return _albums.FirstOrDefault(a => a.Id == id);
    }

    public async Task<Song?> GetSongByIdAsync(Guid id)
    {
        await Task.Delay(50);
        return _songs.FirstOrDefault(s => s.Id == id);
    }

    public async Task<Playlist?> GetPlaylistByIdAsync(Guid id)
    {
        await Task.Delay(50);
        return _playlists.FirstOrDefault(p => p.Id == id);
    }

    private List<Artist> GenerateFakeArtists()
    {
        var artists = new List<Artist>
        {            new Artist 
            { 
                Id = Guid.NewGuid(), 
                Name = "Metallica", 
                Country = "United States",
                FormedYear = 1981,
                Genres = new List<string> { "Thrash Metal", "Heavy Metal" },
                Bio = "Metallica is an American heavy metal band formed in 1981 in Los Angeles by drummer Lars Ulrich and guitarist/vocalist James Hetfield.",
                ImageUrl = "https://www.metal-archives.com/images/1/8/2/5/18254_photo.jpg"
            },
            new Artist 
            { 
                Id = Guid.NewGuid(), 
                Name = "Iron Maiden", 
                Country = "United Kingdom",
                FormedYear = 1975,
                Genres = new List<string> { "Heavy Metal", "NWOBHM" },
                Bio = "Iron Maiden are an English heavy metal band formed in Leyton, East London, in 1975 by bassist and primary songwriter Steve Harris.",
                ImageUrl = "https://www.metal-archives.com/images/7/1/71_photo.jpg"
            },
            new Artist 
            { 
                Id = Guid.NewGuid(), 
                Name = "Black Sabbath", 
                Country = "United Kingdom",
                FormedYear = 1968,
                Genres = new List<string> { "Heavy Metal", "Doom Metal" },
                Bio = "Black Sabbath were an English rock band formed in Birmingham in 1968 by guitarist Tony Iommi, drummer Bill Ward, bassist Geezer Butler and vocalist Ozzy Osbourne.",
                ImageUrl = "https://www.metal-archives.com/images/2/8/1/281_photo.jpg"
            },
            new Artist 
            { 
                Id = Guid.NewGuid(), 
                Name = "Slayer", 
                Country = "United States",
                FormedYear = 1981,
                Genres = new List<string> { "Thrash Metal", "Death Metal" },
                Bio = "Slayer was an American thrash metal band from Huntington Park, California, formed in 1981 by guitarists Kerry King and Jeff Hanneman.",
                ImageUrl = "https://www.metal-archives.com/images/4/5/6/456_photo.jpg"
            },
            new Artist 
            { 
                Id = Guid.NewGuid(), 
                Name = "Megadeth", 
                Country = "United States",
                FormedYear = 1983,
                Genres = new List<string> { "Thrash Metal", "Speed Metal" },
                Bio = "Megadeth is an American thrash metal band formed in Los Angeles in 1983 by vocalist/guitarist Dave Mustaine.",
                ImageUrl = "https://www.metal-archives.com/images/2/6/2/262_photo.jpg"
            },
            new Artist 
            { 
                Id = Guid.NewGuid(), 
                Name = "Judas Priest", 
                Country = "United Kingdom",
                FormedYear = 1969,
                Genres = new List<string> { "Heavy Metal", "Speed Metal" },
                Bio = "Judas Priest are an English heavy metal band formed in Birmingham in 1969.",
                ImageUrl = "https://www.metal-archives.com/images/2/5/2/252_photo.jpg"
            }
        };

        return artists;
    }

    private List<Album> GenerateFakeAlbums()
    {
        var albums = new List<Album>();
        var artists = GenerateFakeArtists();

        foreach (var artist in artists)
        {
            switch (artist.Name)
            {                case "Metallica":
                    albums.AddRange(new[]
                    {
                        new Album { Id = Guid.NewGuid(), Title = "Master of Puppets", ArtistId = artist.Id, ArtistName = artist.Name, ReleaseYear = 1986, CoverImageUrl = "https://www.metal-archives.com/images/5/5/6/8/556873.jpg" },
                        new Album { Id = Guid.NewGuid(), Title = "Ride the Lightning", ArtistId = artist.Id, ArtistName = artist.Name, ReleaseYear = 1984, CoverImageUrl = "https://www.metal-archives.com/images/5/5/6/8/556872.jpg" },
                        new Album { Id = Guid.NewGuid(), Title = "The Black Album", ArtistId = artist.Id, ArtistName = artist.Name, ReleaseYear = 1991, CoverImageUrl = "https://www.metal-archives.com/images/5/5/6/8/556876.jpg" }
                    });
                    break;
                case "Iron Maiden":
                    albums.AddRange(new[]
                    {
                        new Album { Id = Guid.NewGuid(), Title = "The Number of the Beast", ArtistId = artist.Id, ArtistName = artist.Name, ReleaseYear = 1982, CoverImageUrl = "https://www.metal-archives.com/images/7/1/1/4/711484.jpg" },
                        new Album { Id = Guid.NewGuid(), Title = "Powerslave", ArtistId = artist.Id, ArtistName = artist.Name, ReleaseYear = 1984, CoverImageUrl = "https://www.metal-archives.com/images/7/1/1/4/711486.jpg" }
                    });
                    break;
                case "Black Sabbath":
                    albums.AddRange(new[]
                    {
                        new Album { Id = Guid.NewGuid(), Title = "Paranoid", ArtistId = artist.Id, ArtistName = artist.Name, ReleaseYear = 1970, CoverImageUrl = "https://www.metal-archives.com/images/4/9/0/490.jpg" },
                        new Album { Id = Guid.NewGuid(), Title = "Black Sabbath", ArtistId = artist.Id, ArtistName = artist.Name, ReleaseYear = 1970, CoverImageUrl = "https://www.metal-archives.com/images/4/8/9/489.jpg" }
                    });
                    break;                case "Slayer":
                    albums.Add(new Album { Id = Guid.NewGuid(), Title = "Reign in Blood", ArtistId = artist.Id, ArtistName = artist.Name, ReleaseYear = 1986, CoverImageUrl = "https://www.metal-archives.com/images/3/0/4/7/30477.jpg" });
                    break;
                case "Megadeth":
                    albums.Add(new Album { Id = Guid.NewGuid(), Title = "Peace Sells... but Who's Buying?", ArtistId = artist.Id, ArtistName = artist.Name, ReleaseYear = 1986, CoverImageUrl = "https://www.metal-archives.com/images/1/6/8/9/168923.jpg" });
                    break;
                case "Judas Priest":
                    albums.Add(new Album { Id = Guid.NewGuid(), Title = "British Steel", ArtistId = artist.Id, ArtistName = artist.Name, ReleaseYear = 1980, CoverImageUrl = "https://www.metal-archives.com/images/7/9/2/2/79223.jpg" });
                    break;
                default:
                    albums.Add(new Album { Id = Guid.NewGuid(), Title = $"{artist.Name} Greatest Hits", ArtistId = artist.Id, ArtistName = artist.Name, ReleaseYear = 2000, CoverImageUrl = "https://images.unsplash.com/photo-1493225457124-a3eb161ffa5f?w=300&h=300&fit=crop" });
                    break;
            }
        }

        return albums;
    }

    private List<Song> GenerateFakeSongs()
    {
        var songs = new List<Song>();
        var albums = GenerateFakeAlbums();

        foreach (var album in albums)
        {
            switch (album.Title)
            {
                case "Master of Puppets":
                    songs.AddRange(new[]
                    {
                        new Song { Id = Guid.NewGuid(), Title = "Battery", TrackNumber = 1, Duration = TimeSpan.FromMinutes(5.13), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Master of Puppets", TrackNumber = 2, Duration = TimeSpan.FromMinutes(8.35), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "The Thing That Should Not Be", TrackNumber = 3, Duration = TimeSpan.FromMinutes(6.36), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Welcome Home (Sanitarium)", TrackNumber = 4, Duration = TimeSpan.FromMinutes(6.27), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName }
                    });
                    break;
                case "The Number of the Beast":
                    songs.AddRange(new[]
                    {
                        new Song { Id = Guid.NewGuid(), Title = "Invaders", TrackNumber = 1, Duration = TimeSpan.FromMinutes(3.24), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Children of the Damned", TrackNumber = 2, Duration = TimeSpan.FromMinutes(4.35), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "The Number of the Beast", TrackNumber = 3, Duration = TimeSpan.FromMinutes(4.50), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Run to the Hills", TrackNumber = 4, Duration = TimeSpan.FromMinutes(3.54), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName }
                    });
                    break;
                case "Paranoid":
                    songs.AddRange(new[]
                    {
                        new Song { Id = Guid.NewGuid(), Title = "War Pigs", TrackNumber = 1, Duration = TimeSpan.FromMinutes(7.57), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Paranoid", TrackNumber = 2, Duration = TimeSpan.FromMinutes(2.48), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Planet Caravan", TrackNumber = 3, Duration = TimeSpan.FromMinutes(4.32), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },                        new Song { Id = Guid.NewGuid(), Title = "Iron Man", TrackNumber = 4, Duration = TimeSpan.FromMinutes(5.56), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName }
                    });
                    break;
                case "Reign in Blood":
                    songs.AddRange(new[]
                    {
                        new Song { Id = Guid.NewGuid(), Title = "Angel of Death", TrackNumber = 1, Duration = TimeSpan.FromMinutes(4.51), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Piece by Piece", TrackNumber = 2, Duration = TimeSpan.FromMinutes(2.02), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Necrophobic", TrackNumber = 3, Duration = TimeSpan.FromMinutes(1.40), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Raining Blood", TrackNumber = 4, Duration = TimeSpan.FromMinutes(4.17), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName }
                    });
                    break;
                case "Peace Sells... but Who's Buying?":
                    songs.AddRange(new[]
                    {
                        new Song { Id = Guid.NewGuid(), Title = "Wake Up Dead", TrackNumber = 1, Duration = TimeSpan.FromMinutes(3.40), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "The Conjuring", TrackNumber = 2, Duration = TimeSpan.FromMinutes(5.09), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Peace Sells", TrackNumber = 3, Duration = TimeSpan.FromMinutes(4.04), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Devils Island", TrackNumber = 4, Duration = TimeSpan.FromMinutes(5.07), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName }
                    });
                    break;
                case "British Steel":
                    songs.AddRange(new[]
                    {
                        new Song { Id = Guid.NewGuid(), Title = "Rapid Fire", TrackNumber = 1, Duration = TimeSpan.FromMinutes(4.08), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Metal Gods", TrackNumber = 2, Duration = TimeSpan.FromMinutes(4.00), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Breaking the Law", TrackNumber = 3, Duration = TimeSpan.FromMinutes(2.35), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName },
                        new Song { Id = Guid.NewGuid(), Title = "Living After Midnight", TrackNumber = 4, Duration = TimeSpan.FromMinutes(3.31), AlbumId = album.Id, AlbumTitle = album.Title, ArtistId = album.ArtistId, ArtistName = album.ArtistName }
                    });
                    break;
                default:
                    // Generate some generic songs for other albums
                    for (int i = 1; i <= 5; i++)
                    {
                        songs.Add(new Song 
                        { 
                            Id = Guid.NewGuid(), 
                            Title = $"Track {i}", 
                            TrackNumber = i, 
                            Duration = TimeSpan.FromMinutes(new Random().Next(3, 8)), 
                            AlbumId = album.Id, 
                            AlbumTitle = album.Title, 
                            ArtistId = album.ArtistId, 
                            ArtistName = album.ArtistName 
                        });
                    }
                    break;
            }
        }

        return songs;
    }

    private List<Playlist> GenerateFakePlaylists()
    {
        var songs = GenerateFakeSongs();
        var random = new Random();        return new List<Playlist>
        {
            new Playlist
            {
                Id = Guid.NewGuid(),
                Name = "Best of Thrash Metal",
                Description = "The ultimate thrash metal collection",
                CreatedAt = DateTime.Now.AddDays(-30),
                UpdatedAt = DateTime.Now.AddDays(-5),
                CoverImageUrl = "https://images.unsplash.com/photo-1493225457124-a3eb161ffa5f?w=300&h=300&fit=crop&q=80",
                Songs = songs.Where(s => s.ArtistName == "Metallica" || s.ArtistName == "Slayer" || s.ArtistName == "Megadeth").ToList()
            },
            new Playlist
            {
                Id = Guid.NewGuid(),
                Name = "Classic Heavy Metal",
                Description = "The classics that defined heavy metal",
                CreatedAt = DateTime.Now.AddDays(-60),
                UpdatedAt = DateTime.Now.AddDays(-10),
                CoverImageUrl = "https://images.unsplash.com/photo-1459749411175-04bf5292ceea?w=300&h=300&fit=crop&q=80",
                Songs = songs.Where(s => s.ArtistName == "Black Sabbath" || s.ArtistName == "Iron Maiden" || s.ArtistName == "Judas Priest").ToList()
            }
        };
    }
}
