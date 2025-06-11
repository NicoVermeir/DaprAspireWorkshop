using Microsoft.EntityFrameworkCore;
using Metalify.Playlist.Api.Models;

namespace Metalify.Playlist.Api.Data;

/// <summary>
/// Entity Framework DbContext for the Playlist microservice
/// </summary>
public class PlaylistDbContext : DbContext
{
    public PlaylistDbContext(DbContextOptions<PlaylistDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet for Playlists
    /// </summary>
    public DbSet<Models.Playlist> Playlists { get; set; }

    /// <summary>
    /// DbSet for PlaylistItems
    /// </summary>
    public DbSet<PlaylistItem> PlaylistItems { get; set; }

    /// <summary>
    /// Configure the model relationships and constraints
    /// </summary>
    /// <param name="modelBuilder">The model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Playlist entity
        modelBuilder.Entity<Models.Playlist>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Description).HasMaxLength(1000);
            entity.Property(p => p.CoverImageUrl).HasMaxLength(500);
            entity.Property(p => p.CreatedBy).IsRequired().HasMaxLength(100);
            entity.Property(p => p.CreatedAt).IsRequired();
            entity.Property(p => p.UpdatedAt).IsRequired();

            // Index for better query performance
            entity.HasIndex(p => p.CreatedBy);
            entity.HasIndex(p => p.IsPublic);
            entity.HasIndex(p => p.CreatedAt);
        });

        // Configure PlaylistItem entity
        modelBuilder.Entity<PlaylistItem>(entity =>
        {
            entity.HasKey(pi => pi.Id);
            entity.Property(pi => pi.PlaylistId).IsRequired();
            entity.Property(pi => pi.SongId).IsRequired();
            entity.Property(pi => pi.Position).IsRequired();
            entity.Property(pi => pi.AddedAt).IsRequired();

            // Configure relationship with Playlist
            entity.HasOne(pi => pi.Playlist)
                  .WithMany(p => p.PlaylistItems)
                  .HasForeignKey(pi => pi.PlaylistId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint to prevent duplicate songs in same playlist
            entity.HasIndex(pi => new { pi.PlaylistId, pi.SongId }).IsUnique();
            
            // Index for position ordering
            entity.HasIndex(pi => new { pi.PlaylistId, pi.Position });
        });
    }

    /// <summary>
    /// Seed initial data for development
    /// </summary>
    /// <returns>Task</returns>
    public async Task SeedDataAsync()
    {
        if (!Playlists.Any())
        {
            var testPlaylist = new Models.Playlist
            {
                Id = Guid.NewGuid(),
                Name = "Test Heavy Metal Playlist",
                Description = "A sample playlist for testing the playlist microservice",
                CoverImageUrl = "https://images.unsplash.com/photo-1493225457124-a3eb161ffa5f?w=300&h=300&fit=crop&q=80",
                IsPublic = true,
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            Playlists.Add(testPlaylist);
            await SaveChangesAsync();
        }
    }
}
