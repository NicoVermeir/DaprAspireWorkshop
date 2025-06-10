using Metalify.Api.DTOs;
using Metalify.Api.Models;
using Metalify.Api.Repositories;

namespace Metalify.Api.Services;

/// <summary>
/// Service implementation for Artist business logic
/// </summary>
public class ArtistService : IArtistService
{
    private readonly IArtistRepository _artistRepository;
    private readonly ILogger<ArtistService> _logger;

    public ArtistService(IArtistRepository artistRepository, ILogger<ArtistService> logger)
    {
        _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(artistRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<ArtistSummaryDto>> GetAllArtistsAsync()
    {
        _logger.LogInformation("Getting all artists");
        var artists = await _artistRepository.GetAllAsync();
        return artists.Select(MapToSummaryDto);
    }

    public async Task<ArtistDto?> GetArtistByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting artist by ID: {ArtistId}", id);
        var artist = await _artistRepository.GetByIdAsync(id);
        return artist != null ? MapToDto(artist) : null;
    }

    public async Task<IEnumerable<ArtistSummaryDto>> GetArtistsByCountryAsync(string country)
    {
        _logger.LogInformation("Getting artists by country: {Country}", country);
        var artists = await _artistRepository.GetByCountryAsync(country);
        return artists.Select(MapToSummaryDto);
    }

    public async Task<IEnumerable<ArtistSummaryDto>> SearchArtistsAsync(string searchTerm)
    {
        _logger.LogInformation("Searching artists with term: {SearchTerm}", searchTerm);
        var artists = await _artistRepository.SearchByNameAsync(searchTerm);
        return artists.Select(MapToSummaryDto);
    }

    private static ArtistDto MapToDto(Artist artist)
    {
        return new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            Country = artist.Country,
            FormedYear = artist.FormedYear,
            Genres = string.IsNullOrEmpty(artist.Genres) 
                ? new List<string>() 
                : artist.Genres.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            Albums = artist.Albums.Select(album => new AlbumSummaryDto
            {
                Id = album.Id,
                Title = album.Title,
                CoverImageUrl = album.CoverImageUrl,
                ReleaseYear = album.ReleaseYear,
                ArtistId = album.ArtistId,
                ArtistName = artist.Name,
                SongCount = album.Songs.Count
            }).ToList()
        };
    }

    private static ArtistSummaryDto MapToSummaryDto(Artist artist)
    {
        return new ArtistSummaryDto
        {
            Id = artist.Id,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Country = artist.Country,
            FormedYear = artist.FormedYear,
            Genres = string.IsNullOrEmpty(artist.Genres) 
                ? new List<string>() 
                : artist.Genres.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
        };
    }
}
