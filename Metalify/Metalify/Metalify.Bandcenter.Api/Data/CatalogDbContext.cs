using Microsoft.EntityFrameworkCore;
using Metalify.Bandcenter.Api.Models;

namespace Metalify.Bandcenter.Api.Data;

/// <summary>
/// Entity Framework DbContext for the Metalify Catalog microservice
/// </summary>
public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet for Bands
    /// </summary>
    public DbSet<Artist> Bands { get; set; }

    /// <summary>
    /// DbSet for Albums
    /// </summary>
    public DbSet<Album> Albums { get; set; }

    /// <summary>
    /// DbSet for Songs
    /// </summary>
    public DbSet<Song> Songs { get; set; }

    /// <summary>
    /// Configure the model relationships and constraints
    /// </summary>
    /// <param name="modelBuilder">The model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Band entity
        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Name).IsRequired().HasMaxLength(200);
            entity.Property(b => b.Country).IsRequired().HasMaxLength(100);
            entity.Property(b => b.Status).IsRequired().HasMaxLength(50);
            entity.Property(b => b.Genres).IsRequired().HasMaxLength(100);
            entity.Property(b => b.Location).HasMaxLength(200);
            entity.Property(b => b.Themes).HasMaxLength(500);
            entity.Property(b => b.Label).HasMaxLength(200);
            entity.Property(b => b.YearsActive).HasMaxLength(100);
            entity.Property(b => b.LogoUrl).HasMaxLength(500);
            entity.Property(b => b.ImageUrl).HasMaxLength(500);
            entity.Property(b => b.CreatedAt).IsRequired();
            entity.Property(b => b.UpdatedAt).IsRequired();

            // Indexes for better query performance
            entity.HasIndex(b => b.Name);
            entity.HasIndex(b => b.Country);
            entity.HasIndex(b => b.Genres);
            entity.HasIndex(b => b.Status);
            entity.HasIndex(b => b.FormedYear);
        });

        // Configure Album entity
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Title).IsRequired().HasMaxLength(200);
            entity.Property(a => a.AlbumType).IsRequired().HasMaxLength(50);
            entity.Property(a => a.CoverImageUrl).HasMaxLength(500);
            entity.Property(a => a.Label).HasMaxLength(200);
            entity.Property(a => a.CatalogNumber).HasMaxLength(100);
            entity.Property(a => a.Format).HasMaxLength(100);
            entity.Property(a => a.CreatedAt).IsRequired();
            entity.Property(a => a.UpdatedAt).IsRequired();

            // Foreign key relationship
            entity.HasOne(a => a.Artist)
                .WithMany(b => b.Albums)
                .HasForeignKey(a => a.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for better query performance
            entity.HasIndex(a => a.Title);
            entity.HasIndex(a => a.ReleaseYear);
            entity.HasIndex(a => a.AlbumType);
            entity.HasIndex(a => a.ArtistId);
        });

        // Configure Song entity
        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Title).IsRequired().HasMaxLength(200);
            entity.Property(s => s.CreatedAt).IsRequired();
            entity.Property(s => s.UpdatedAt).IsRequired();

            // Foreign key relationships
            entity.HasOne(s => s.Album)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.Artist)
                .WithMany()
                .HasForeignKey(s => s.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for better query performance
            entity.HasIndex(s => s.Title);
            entity.HasIndex(s => s.TrackNumber);
            entity.HasIndex(s => s.AlbumId);
            entity.HasIndex(s => s.ArtistId);

            // Composite index for album track ordering
            entity.HasIndex(s => new { s.AlbumId, s.TrackNumber });
        });

        // Seed data
        SeedData(modelBuilder);
    }

    /// <summary>
    /// Seed initial data for development with real metal bands from Metal Archives
    /// </summary>
    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Artists with real metal bands from Metal Archives inspiration
        var artists = new[]
        {
            new Artist
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Iron Maiden",
                ImageUrl = "https://www.metal-archives.com/images/1/6/4/164_photo.jpg?5628",
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
                ImageUrl = "https://www.metal-archives.com/images/1/2/5/125_photo.jpg?2329",
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
                ImageUrl = "https://www.metal-archives.com/images/1/3/1/131_photo.jpg?1425",
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
                ImageUrl = "https://www.metal-archives.com/images/1/5/4/154_photo.jpg?8836",
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
                ImageUrl = "https://www.metal-archives.com/images/1/4/4/144_artist.jpg",
                Bio = "American thrash metal band formed in Los Angeles in 1983 by guitarist Dave Mustaine.",
                Country = "United States",
                FormedYear = 1983,
                Genres = "Thrash Metal,Speed Metal",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Artist
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                Name = "Slayer",
                ImageUrl = "https://www.metal-archives.com/images/7/2/72_photo.jpg",
                Bio =
                    "American thrash metal band formed in Huntington Park, California in 1981 by guitarists Kerry King and Jeff Hanneman.",
                Country = "United States",
                FormedYear = 1981,
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
                CoverImageUrl = "https://www.metal-archives.com/images/1/6/4/164_logo.jpg",
                ReleaseYear = 1982,
                ArtistId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Album
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Title = "Powerslave",
                CoverImageUrl = "https://www.metal-archives.com/images/1/6/4/164_photo.jpg",
                ReleaseYear = 1984,
                ArtistId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }, // Metallica Albums
            new Album
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Title = "Master of Puppets",
                CoverImageUrl = "https://www.metal-archives.com/images/1/2/5/125_logo.jpg",
                ReleaseYear = 1986,
                ArtistId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Album
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Title = "Ride the Lightning",
                CoverImageUrl = "https://www.metal-archives.com/images/1/2/5/125_photo.jpg",
                ReleaseYear = 1984,
                ArtistId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }, // Black Sabbath Album
            new Album
            {
                Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "Paranoid",
                CoverImageUrl = "https://www.metal-archives.com/images/1/3/1/131_photo.jpg",
                ReleaseYear = 1970,
                ArtistId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Slayer Albums
            new Album
            {
                Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                Title = "Reign in Blood",
                CoverImageUrl = "https://www.metal-archives.com/images/2/1/2/212.jpg",
                ReleaseYear = 1986,
                ArtistId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Album
            {
                Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                Title = "Seasons in the Abyss",
                CoverImageUrl = "https://www.metal-archives.com/images/2/1/8/218.jpg",
                ReleaseYear = 1990,
                ArtistId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Album
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                Title = "South of Heaven",
                CoverImageUrl = "https://www.metal-archives.com/images/2/1/4/214.jpg",
                ReleaseYear = 1988,
                ArtistId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Slayer songs
            new Song
            {
                Id = Guid.Parse("66666666-ffff-ffff-ffff-666666666661"),
                Title = "Angel of Death",
                TrackNumber = 1,
                Duration = TimeSpan.FromMinutes(4).Add(TimeSpan.FromSeconds(51)),
                AlbumId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                ArtistId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("66666666-ffff-ffff-ffff-666666666662"),
                Title = "Raining Blood",
                TrackNumber = 10,
                Duration = TimeSpan.FromMinutes(4).Add(TimeSpan.FromSeconds(17)),
                AlbumId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                ArtistId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("66666666-7777-7777-7777-666666666663"),
                Title = "Seasons in the Abyss",
                TrackNumber = 8,
                Duration = TimeSpan.FromMinutes(6).Add(TimeSpan.FromSeconds(6)),
                AlbumId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                ArtistId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("66666666-8888-8888-8888-666666666664"),
                Title = "South of Heaven",
                TrackNumber = 1,
                Duration = TimeSpan.FromMinutes(4).Add(TimeSpan.FromSeconds(58)),
                AlbumId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                ArtistId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        modelBuilder.Entity<Song>().HasData(songs);
    }

    /// <summary>
    /// Seed additional data for development (called after database creation)
    /// </summary>
    /// <returns>Task</returns>
    public async Task SeedDataAsync()
    {
        if (!Bands.Any())
        {
            // Additional bands can be added here if needed
            await SaveChangesAsync();
        }
    }
}