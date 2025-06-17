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
    public DbSet<Band> Bands { get; set; }

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
        modelBuilder.Entity<Band>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Name).IsRequired().HasMaxLength(200);
            entity.Property(b => b.Country).IsRequired().HasMaxLength(100);
            entity.Property(b => b.Status).IsRequired().HasMaxLength(50);
            entity.Property(b => b.Genre).IsRequired().HasMaxLength(100);
            entity.Property(b => b.Location).HasMaxLength(200);
            entity.Property(b => b.Themes).HasMaxLength(500);
            entity.Property(b => b.Label).HasMaxLength(200);
            entity.Property(b => b.YearsActive).HasMaxLength(100);
            entity.Property(b => b.LogoUrl).HasMaxLength(500);
            entity.Property(b => b.PhotoUrl).HasMaxLength(500);
            entity.Property(b => b.CreatedAt).IsRequired();
            entity.Property(b => b.UpdatedAt).IsRequired();

            // Indexes for better query performance
            entity.HasIndex(b => b.Name);
            entity.HasIndex(b => b.Country);
            entity.HasIndex(b => b.Genre);
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
            entity.HasOne(a => a.Band)
                  .WithMany(b => b.Albums)
                  .HasForeignKey(a => a.BandId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Indexes for better query performance
            entity.HasIndex(a => a.Title);
            entity.HasIndex(a => a.ReleaseYear);
            entity.HasIndex(a => a.AlbumType);
            entity.HasIndex(a => a.BandId);
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

            entity.HasOne(s => s.Band)
                  .WithMany()
                  .HasForeignKey(s => s.BandId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Indexes for better query performance
            entity.HasIndex(s => s.Title);
            entity.HasIndex(s => s.TrackNumber);
            entity.HasIndex(s => s.AlbumId);
            entity.HasIndex(s => s.BandId);

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
        // Seed Bands with real data from Metal Archives
        var bands = new[]
        {
            new Band
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Iron Maiden",
                Country = "United Kingdom",
                Location = "London, England",
                Status = "Active",
                FormedYear = 1975,
                Genre = "Heavy Metal",
                Themes = "Literature, History, War, Religion, Society",
                Label = "Parlophone",
                YearsActive = "1975-present",
                LogoUrl = "https://www.metal-archives.com/images/7/6/76_logo.png?4616",
                PhotoUrl = "https://www.metal-archives.com/images/7/6/76_photo.jpg?5721",
                Biography = "British heavy metal band formed in London in 1975 by bassist Steve Harris. Known for their elaborate stage shows, historical and literary lyrical themes, and the operatic vocal style of Bruce Dickinson.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Band
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Metallica",
                Country = "United States",
                Location = "San Rafael, California",
                Status = "Active",
                FormedYear = 1981,
                Genre = "Thrash Metal, Heavy Metal",
                Themes = "Death, War, Madness, Monsters, Religion",
                Label = "Blackened Recordings",
                YearsActive = "1981-present",
                LogoUrl = "https://www.metal-archives.com/images/1/2/5/125_logo.png?4844",
                PhotoUrl = "https://www.metal-archives.com/images/1/2/5/125_photo.jpg?2329",
                Biography = "American heavy metal band formed in Los Angeles, California. The band was formed in 1981 when vocalist/guitarist James Hetfield responded to an advertisement posted by drummer Lars Ulrich in a local newspaper.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Band
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Black Sabbath",
                Country = "United Kingdom",
                Location = "Birmingham, England",
                Status = "Split-up",
                FormedYear = 1968,
                Genre = "Doom Metal, Heavy Metal",
                Themes = "Occultism, Horror, Politics, War, Drugs",
                Label = "BMG",
                YearsActive = "1968-2017",
                LogoUrl = "https://www.metal-archives.com/images/4/0/40_logo.png?4639",
                PhotoUrl = "https://www.metal-archives.com/images/4/0/40_photo.jpg?1425",
                Biography = "English rock band formed in Birmingham in 1968. They are often cited as pioneers of heavy metal music. The classic line-up included Ozzy Osbourne, Tony Iommi, Geezer Butler, and Bill Ward.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Band
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Judas Priest",
                Country = "United Kingdom",
                Location = "Birmingham, England",
                Status = "Active",
                FormedYear = 1969,
                Genre = "Heavy Metal",
                Themes = "Metal, Religion, Politics, Social issues",
                Label = "Columbia Records",
                YearsActive = "1969-present",
                LogoUrl = "https://www.metal-archives.com/images/2/5/4/254_logo.png?4748",
                PhotoUrl = "https://www.metal-archives.com/images/2/5/4/254_photo.jpg?8836",
                Biography = "English heavy metal band formed in Birmingham. The band have sold over 50 million albums and are frequently ranked as one of the greatest metal bands of all time.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Band
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Name = "Megadeth",
                Country = "United States",
                Location = "Los Angeles, California",
                Status = "Active",
                FormedYear = 1983,
                Genre = "Thrash Metal, Speed Metal",
                Themes = "Politics, War, Religion, Death, Alienation",
                Label = "UMe",
                YearsActive = "1983-2002, 2004-present",
                LogoUrl = "https://www.metal-archives.com/images/1/2/8/128_logo.png?4851",
                PhotoUrl = "https://www.metal-archives.com/images/1/2/8/128_photo.jpg?1527",
                Biography = "American thrash metal band formed in Los Angeles by guitarist Dave Mustaine after he was kicked out of Metallica. Along with Metallica, Anthrax, and Slayer, Megadeth is one of the 'big four' thrash metal bands.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        modelBuilder.Entity<Band>().HasData(bands);

        // Seed Albums with real data
        var albums = new[]
        {
            // Iron Maiden Albums
            new Album
            {
                Id = Guid.Parse("11111111-aaaa-1111-aaaa-111111111111"),
                Title = "The Number of the Beast",
                AlbumType = "Full-length",
                ReleaseYear = 1982,
                CoverImageUrl = "https://www.metal-archives.com/images/2/3/6/8/23688.jpg?4616",
                Label = "EMI",
                CatalogNumber = "EMC 3400",
                Format = "CD, Vinyl",
                TotalDuration = TimeSpan.FromMinutes(39).Add(TimeSpan.FromSeconds(36)),
                Notes = "Bruce Dickinson's first album with Iron Maiden",
                BandId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Album
            {
                Id = Guid.Parse("22222222-bbbb-2222-bbbb-222222222222"),
                Title = "Powerslave",
                AlbumType = "Full-length",
                ReleaseYear = 1984,
                CoverImageUrl = "https://www.metal-archives.com/images/2/3/7/2/23726.jpg?4616",
                Label = "EMI",
                CatalogNumber = "POWER 1",
                Format = "CD, Vinyl",
                TotalDuration = TimeSpan.FromMinutes(51).Add(TimeSpan.FromSeconds(19)),
                Notes = "Features the epic 'Rime of the Ancient Mariner'",
                BandId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Metallica Albums
            new Album
            {
                Id = Guid.Parse("33333333-cccc-3333-cccc-333333333333"),
                Title = "Master of Puppets",
                AlbumType = "Full-length",
                ReleaseYear = 1986,
                CoverImageUrl = "https://www.metal-archives.com/images/2/1/4/214.jpg?4844",
                Label = "Music for Nations",
                CatalogNumber = "MFN 60",
                Format = "CD, Vinyl",
                TotalDuration = TimeSpan.FromMinutes(54).Add(TimeSpan.FromSeconds(46)),
                Notes = "Often considered one of the greatest metal albums of all time",
                BandId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Album
            {
                Id = Guid.Parse("44444444-dddd-4444-dddd-444444444444"),
                Title = "Ride the Lightning",
                AlbumType = "Full-length",
                ReleaseYear = 1984,
                CoverImageUrl = "https://www.metal-archives.com/images/2/1/3/213.jpg?4844",
                Label = "Music for Nations",
                CatalogNumber = "MFN 27",
                Format = "CD, Vinyl",
                TotalDuration = TimeSpan.FromMinutes(47).Add(TimeSpan.FromSeconds(26)),
                Notes = "First album to feature Kirk Hammett on lead guitar",
                BandId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Black Sabbath Albums
            new Album
            {
                Id = Guid.Parse("55555555-eeee-5555-eeee-555555555555"),
                Title = "Paranoid",
                AlbumType = "Full-length",
                ReleaseYear = 1970,
                CoverImageUrl = "https://www.metal-archives.com/images/4/6/46.jpg?4639",
                Label = "Vertigo",
                CatalogNumber = "6360 011",
                Format = "CD, Vinyl",
                TotalDuration = TimeSpan.FromMinutes(42).Add(TimeSpan.FromSeconds(8)),
                Notes = "Features classic tracks like 'Paranoid' and 'Iron Man'",
                BandId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Judas Priest Albums
            new Album
            {
                Id = Guid.Parse("66666666-ffff-6666-ffff-666666666666"),
                Title = "British Steel",
                AlbumType = "Full-length",
                ReleaseYear = 1980,
                CoverImageUrl = "https://www.metal-archives.com/images/3/4/0/340.jpg?4748",
                Label = "Columbia",
                CatalogNumber = "FC 36443",
                Format = "CD, Vinyl",
                TotalDuration = TimeSpan.FromMinutes(38).Add(TimeSpan.FromSeconds(8)),
                Notes = "Features 'Breaking the Law' and 'Living After Midnight'",
                BandId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Megadeth Albums
            new Album
            {
                Id = Guid.Parse("77777777-aaaa-7777-aaaa-777777777777"),
                Title = "Peace Sells... but Who's Buying?",
                AlbumType = "Full-length",
                ReleaseYear = 1986,
                CoverImageUrl = "https://www.metal-archives.com/images/2/2/8/228.jpg?4851",
                Label = "Capitol Records",
                CatalogNumber = "C1-46148",
                Format = "CD, Vinyl",
                TotalDuration = TimeSpan.FromMinutes(36).Add(TimeSpan.FromSeconds(4)),
                Notes = "Second studio album and first to feature Chris Poland and Gar Samuelson",
                BandId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        modelBuilder.Entity<Album>().HasData(albums);

        // Seed Songs with real data
        var songs = new[]
        {
            // The Number of the Beast songs
            new Song
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Title = "Invaders",
                TrackNumber = 1,
                Duration = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(24)),
                Lyrics = "Longboats have been sighted and the evidence of war has begun...",                Notes = "Opening track about Viking invasions",
                AlbumId = Guid.Parse("11111111-aaaa-1111-aaaa-111111111111"),
                BandId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                Title = "Children of the Damned",
                TrackNumber = 2,
                Duration = TimeSpan.FromMinutes(4).Add(TimeSpan.FromSeconds(35)),
                Lyrics = "He's walking like a small child, but watch his eyes burn you away...",                Notes = "Based on the film 'Village of the Damned'",
                AlbumId = Guid.Parse("11111111-aaaa-1111-aaaa-111111111111"),
                BandId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                Title = "The Number of the Beast",
                TrackNumber = 6,
                Duration = TimeSpan.FromMinutes(4).Add(TimeSpan.FromSeconds(50)),
                Lyrics = "Woe to you, oh earth and sea, for the Devil sends the beast with wrath...",                Notes = "Title track inspired by a nightmare and the Book of Revelation",
                AlbumId = Guid.Parse("11111111-aaaa-1111-aaaa-111111111111"),
                BandId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },

            // Master of Puppets songs
            new Song
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222221"),
                Title = "Battery",
                TrackNumber = 1,
                Duration = TimeSpan.FromMinutes(5).Add(TimeSpan.FromSeconds(13)),
                Lyrics = "Lashing out the action, returning the reaction...",                Notes = "Opens with acoustic guitar before exploding into thrash",
                AlbumId = Guid.Parse("33333333-cccc-3333-cccc-333333333333"),
                BandId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "Master of Puppets",
                TrackNumber = 2,
                Duration = TimeSpan.FromMinutes(8).Add(TimeSpan.FromSeconds(35)),
                Lyrics = "End of passion play, crumbling away...",                Notes = "Epic title track about addiction and control",
                AlbumId = Guid.Parse("33333333-cccc-3333-cccc-333333333333"),
                BandId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },

            // Paranoid songs
            new Song
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333331"),
                Title = "War Pigs",
                TrackNumber = 1,
                Duration = TimeSpan.FromMinutes(7).Add(TimeSpan.FromSeconds(57)),
                Lyrics = "Generals gathered in their masses, just like witches at black masses...",                Notes = "Anti-war anthem and album opener",
                AlbumId = Guid.Parse("55555555-eeee-5555-eeee-555555555555"),
                BandId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333332"),
                Title = "Paranoid",
                TrackNumber = 2,
                Duration = TimeSpan.FromMinutes(2).Add(TimeSpan.FromSeconds(48)),
                Lyrics = "Finished with my woman 'cause she couldn't help me with my mind...",                Notes = "Title track and biggest hit single",
                AlbumId = Guid.Parse("55555555-eeee-5555-eeee-555555555555"),
                BandId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Title = "Iron Man",
                TrackNumber = 4,
                Duration = TimeSpan.FromMinutes(5).Add(TimeSpan.FromSeconds(55)),
                Lyrics = "I am Iron Man, has he lost his mind? Can he see or is he blind?",                Notes = "One of the most famous metal riffs of all time",
                AlbumId = Guid.Parse("55555555-eeee-5555-eeee-555555555555"),
                BandId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },

            // British Steel songs
            new Song
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444441"),
                Title = "Breaking the Law",
                TrackNumber = 3,
                Duration = TimeSpan.FromMinutes(2).Add(TimeSpan.FromSeconds(35)),
                Lyrics = "There I was completely wasting, out of work and down...",                Notes = "Iconic anthem with memorable music video",
                AlbumId = Guid.Parse("66666666-ffff-6666-ffff-666666666666"),
                BandId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444442"),
                Title = "Living After Midnight",
                TrackNumber = 4,
                Duration = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(31)),
                Lyrics = "Living after midnight, rockin' to the dawn...",                Notes = "Classic party anthem",
                AlbumId = Guid.Parse("66666666-ffff-6666-ffff-666666666666"),
                BandId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },

            // Peace Sells songs
            new Song
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555551"),
                Title = "Wake Up Dead",
                TrackNumber = 1,
                Duration = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(35)),
                Lyrics = "I sneak in my own house, it's four in the morning...",                Notes = "Opening track about domestic troubles",
                AlbumId = Guid.Parse("77777777-aaaa-7777-aaaa-777777777777"),
                BandId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Song
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555552"),
                Title = "Peace Sells",
                TrackNumber = 2,
                Duration = TimeSpan.FromMinutes(4).Add(TimeSpan.FromSeconds(4)),
                Lyrics = "What do you mean I don't believe in God? I talk to him every day...",                Notes = "Title track with iconic bass line and political themes",
                AlbumId = Guid.Parse("77777777-aaaa-7777-aaaa-777777777777"),
                BandId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
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
