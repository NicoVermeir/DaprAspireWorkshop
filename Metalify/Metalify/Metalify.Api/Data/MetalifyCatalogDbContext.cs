using Microsoft.EntityFrameworkCore;
using Metalify.Api.Models;

namespace Metalify.Api.Data;

/// <summary>
/// Entity Framework DbContext for the Metalify catalog database
/// </summary>
public class MetalifyCatalogDbContext : DbContext
{
    public MetalifyCatalogDbContext(DbContextOptions<MetalifyCatalogDbContext> options) : base(options)
    {
    }

    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Song> Songs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Artist configuration
        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Bio).HasMaxLength(2000);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.Genres).HasMaxLength(500);
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Country);
        });

        // Album configuration
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CoverImageUrl).HasMaxLength(500);
            entity.HasIndex(e => e.Title);
            entity.HasIndex(e => e.ReleaseYear);
            
            // Foreign key relationship
            entity.HasOne(a => a.Artist)
                  .WithMany(ar => ar.Albums)
                  .HasForeignKey(a => a.ArtistId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Song configuration
        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.AudioUrl).HasMaxLength(500);
            entity.HasIndex(e => e.Title);
            entity.HasIndex(e => e.TrackNumber);
            
            // Foreign key relationships
            entity.HasOne(s => s.Album)
                  .WithMany(a => a.Songs)
                  .HasForeignKey(s => s.AlbumId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.Artist)
                  .WithMany()
                  .HasForeignKey(s => s.ArtistId)
                  .OnDelete(DeleteBehavior.NoAction); // Prevent multiple cascade paths
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Artists with real metal bands from Metal Archives inspiration
        var artists = new[]
        {
            new Artist
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Iron Maiden",
                ImageUrl = "https://example.com/images/iron-maiden.jpg",
                Bio = "British heavy metal band formed in London in 1975 by bassist and primary songwriter Steve Harris.",
                Country = "United Kingdom",
                FormedYear = 1975,
                Genres = "Heavy Metal,NWOBHM",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Artist
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Metallica",
                ImageUrl = "https://example.com/images/metallica.jpg",
                Bio = "American heavy metal band formed in Los Angeles in 1981.",
                Country = "United States",
                FormedYear = 1981,
                Genres = "Heavy Metal,Thrash Metal",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Artist
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Black Sabbath",
                ImageUrl = "https://example.com/images/black-sabbath.jpg",
                Bio = "English rock band formed in Birmingham in 1968, often cited as pioneers of heavy metal music.",
                Country = "United Kingdom",
                FormedYear = 1968,
                Genres = "Heavy Metal,Doom Metal",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Artist
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Judas Priest",
                ImageUrl = "https://example.com/images/judas-priest.jpg",
                Bio = "English heavy metal band formed in Birmingham in 1969.",
                Country = "United Kingdom",
                FormedYear = 1969,
                Genres = "Heavy Metal,Speed Metal",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Artist
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Name = "Megadeth",
                ImageUrl = "https://example.com/images/megadeth.jpg",
                Bio = "American thrash metal band formed in Los Angeles in 1983 by guitarist Dave Mustaine.",
                Country = "United States",
                FormedYear = 1983,
                Genres = "Thrash Metal,Speed Metal",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        modelBuilder.Entity<Artist>().HasData(artists);

        // Seed Albums
        var albums = new[]
        {
            // Iron Maiden Albums
            new Album
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Title = "The Number of the Beast",
                CoverImageUrl = "https://example.com/covers/number-beast.jpg",
                ReleaseYear = 1982,
                ArtistId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Album
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Title = "Powerslave",
                CoverImageUrl = "https://example.com/covers/powerslave.jpg",
                ReleaseYear = 1984,
                ArtistId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Metallica Albums
            new Album
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Title = "Master of Puppets",
                CoverImageUrl = "https://example.com/covers/master-puppets.jpg",
                ReleaseYear = 1986,
                ArtistId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Album
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Title = "Ride the Lightning",
                CoverImageUrl = "https://example.com/covers/ride-lightning.jpg",
                ReleaseYear = 1984,
                ArtistId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Black Sabbath Album
            new Album
            {
                Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "Paranoid",
                CoverImageUrl = "https://example.com/covers/paranoid.jpg",
                ReleaseYear = 1970,
                ArtistId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        modelBuilder.Entity<Album>().HasData(albums);

        // Seed Songs
        var songs = new[]
        {
            // The Number of the Beast songs
            new Song
            {
                Id = Guid.Parse("11111111-aaaa-aaaa-aaaa-111111111111"),
                Title = "The Number of the Beast",
                TrackNumber = 6,
                Duration = TimeSpan.FromMinutes(4).Add(TimeSpan.FromSeconds(50)),
                AlbumId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                ArtistId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                AudioUrl = "https://example.com/audio/number-beast.mp3",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("11111111-aaaa-aaaa-aaaa-111111111112"),
                Title = "Run to the Hills",
                TrackNumber = 4,
                Duration = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(52)),
                AlbumId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                ArtistId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                AudioUrl = "https://example.com/audio/run-hills.mp3",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Master of Puppets songs
            new Song
            {
                Id = Guid.Parse("22222222-cccc-cccc-cccc-222222222221"),
                Title = "Master of Puppets",
                TrackNumber = 1,
                Duration = TimeSpan.FromMinutes(8).Add(TimeSpan.FromSeconds(35)),
                AlbumId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                ArtistId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                AudioUrl = "https://example.com/audio/master-puppets.mp3",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("22222222-cccc-cccc-cccc-222222222222"),
                Title = "Battery",
                TrackNumber = 2,
                Duration = TimeSpan.FromMinutes(5).Add(TimeSpan.FromSeconds(13)),
                AlbumId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                ArtistId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                AudioUrl = "https://example.com/audio/battery.mp3",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Paranoid songs
            new Song
            {
                Id = Guid.Parse("33333333-eeee-eeee-eeee-333333333331"),
                Title = "Paranoid",
                TrackNumber = 2,
                Duration = TimeSpan.FromMinutes(2).Add(TimeSpan.FromSeconds(48)),
                AlbumId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                ArtistId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                AudioUrl = "https://example.com/audio/paranoid.mp3",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("33333333-eeee-eeee-eeee-333333333332"),
                Title = "Iron Man",
                TrackNumber = 4,
                Duration = TimeSpan.FromMinutes(5).Add(TimeSpan.FromSeconds(55)),
                AlbumId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                ArtistId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                AudioUrl = "https://example.com/audio/iron-man.mp3",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        modelBuilder.Entity<Song>().HasData(songs);
    }
}
