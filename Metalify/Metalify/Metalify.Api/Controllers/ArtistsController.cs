using Microsoft.AspNetCore.Mvc;
using Metalify.Api.DTOs;
using Metalify.Api.Services;

namespace Metalify.Api.Controllers;

/// <summary>
/// REST API controller for Artist operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ArtistsController : ControllerBase
{
    private readonly IArtistService _artistService;
    private readonly ILogger<ArtistsController> _logger;

    public ArtistsController(IArtistService artistService, ILogger<ArtistsController> logger)
    {
        _artistService = artistService ?? throw new ArgumentNullException(nameof(artistService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all artists
    /// </summary>
    /// <returns>List of all artists</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<ArtistSummaryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ArtistSummaryDto>>> GetAllArtists()
    {
        try
        {
            _logger.LogInformation("GET /api/artists - Retrieving all artists");
            var artists = await _artistService.GetAllArtistsAsync();
            return Ok(artists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all artists");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving artists");
        }
    }

    /// <summary>
    /// Get artist by ID
    /// </summary>
    /// <param name="id">Artist ID</param>
    /// <returns>Artist details with albums</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<ArtistDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ArtistDto>> GetArtistById(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("GET /api/artists/{Id} - Invalid artist ID: {Id}", id);
                return BadRequest("Artist ID cannot be empty");
            }

            _logger.LogInformation("GET /api/artists/{Id} - Retrieving artist", id);
            var artist = await _artistService.GetArtistByIdAsync(id);
            
            if (artist == null)
            {
                _logger.LogInformation("Artist not found with ID: {Id}", id);
                return NotFound($"Artist with ID {id} not found");
            }

            return Ok(artist);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving artist with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the artist");
        }
    }

    /// <summary>
    /// Search artists by name
    /// </summary>
    /// <param name="q">Search query</param>
    /// <returns>List of matching artists</returns>
    [HttpGet("search")]
    [ProducesResponseType<IEnumerable<ArtistSummaryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ArtistSummaryDto>>> SearchArtists([FromQuery] string q)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                _logger.LogWarning("GET /api/artists/search - Empty search query");
                return BadRequest("Search query cannot be empty");
            }

            _logger.LogInformation("GET /api/artists/search?q={Query} - Searching artists", q);
            var artists = await _artistService.SearchArtistsAsync(q);
            return Ok(artists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching artists with query: {Query}", q);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching artists");
        }
    }

    /// <summary>
    /// Get artists by country
    /// </summary>
    /// <param name="country">Country name</param>
    /// <returns>List of artists from the specified country</returns>
    [HttpGet("country/{country}")]
    [ProducesResponseType<IEnumerable<ArtistSummaryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ArtistSummaryDto>>> GetArtistsByCountry(string country)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                _logger.LogWarning("GET /api/artists/country/{Country} - Empty country", country);
                return BadRequest("Country cannot be empty");
            }

            _logger.LogInformation("GET /api/artists/country/{Country} - Retrieving artists by country", country);
            var artists = await _artistService.GetArtistsByCountryAsync(country);
            return Ok(artists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving artists by country: {Country}", country);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving artists by country");
        }
    }
}
