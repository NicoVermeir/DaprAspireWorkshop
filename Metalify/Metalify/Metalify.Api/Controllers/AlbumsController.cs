using Microsoft.AspNetCore.Mvc;
using Metalify.Api.DTOs;
using Metalify.Api.Services;

namespace Metalify.Api.Controllers;

/// <summary>
/// REST API controller for Album operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AlbumsController : ControllerBase
{
    private readonly IAlbumService _albumService;
    private readonly ILogger<AlbumsController> _logger;

    public AlbumsController(IAlbumService albumService, ILogger<AlbumsController> logger)
    {
        _albumService = albumService ?? throw new ArgumentNullException(nameof(albumService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all albums
    /// </summary>
    /// <returns>List of all albums</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<AlbumSummaryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<AlbumSummaryDto>>> GetAllAlbums()
    {
        try
        {
            _logger.LogInformation("GET /api/albums - Retrieving all albums");
            var albums = await _albumService.GetAllAlbumsAsync();
            return Ok(albums);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all albums");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving albums");
        }
    }

    /// <summary>
    /// Get album by ID
    /// </summary>
    /// <param name="id">Album ID</param>
    /// <returns>Album details with songs</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<AlbumDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AlbumDto>> GetAlbumById(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("GET /api/albums/{Id} - Invalid album ID: {Id}", id);
                return BadRequest("Album ID cannot be empty");
            }

            _logger.LogInformation("GET /api/albums/{Id} - Retrieving album", id);
            var album = await _albumService.GetAlbumByIdAsync(id);
            
            if (album == null)
            {
                _logger.LogInformation("Album not found with ID: {Id}", id);
                return NotFound($"Album with ID {id} not found");
            }

            return Ok(album);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving album with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the album");
        }
    }

    /// <summary>
    /// Get albums by artist
    /// </summary>
    /// <param name="artistId">Artist ID</param>
    /// <returns>List of albums by the specified artist</returns>
    [HttpGet("artist/{artistId:guid}")]
    [ProducesResponseType<IEnumerable<AlbumSummaryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<AlbumSummaryDto>>> GetAlbumsByArtist(Guid artistId)
    {
        try
        {
            if (artistId == Guid.Empty)
            {
                _logger.LogWarning("GET /api/albums/artist/{ArtistId} - Invalid artist ID: {ArtistId}", artistId);
                return BadRequest("Artist ID cannot be empty");
            }

            _logger.LogInformation("GET /api/albums/artist/{ArtistId} - Retrieving albums by artist", artistId);
            var albums = await _albumService.GetAlbumsByArtistIdAsync(artistId);
            return Ok(albums);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving albums by artist ID: {ArtistId}", artistId);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving albums by artist");
        }
    }

    /// <summary>
    /// Get albums by release year
    /// </summary>
    /// <param name="year">Release year</param>
    /// <returns>List of albums released in the specified year</returns>
    [HttpGet("year/{year:int}")]
    [ProducesResponseType<IEnumerable<AlbumSummaryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<AlbumSummaryDto>>> GetAlbumsByYear(int year)
    {
        try
        {
            if (year < 1900 || year > DateTime.Now.Year + 1)
            {
                _logger.LogWarning("GET /api/albums/year/{Year} - Invalid year: {Year}", year);
                return BadRequest("Year must be between 1900 and current year + 1");
            }

            _logger.LogInformation("GET /api/albums/year/{Year} - Retrieving albums by year", year);
            var albums = await _albumService.GetAlbumsByYearAsync(year);
            return Ok(albums);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving albums by year: {Year}", year);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving albums by year");
        }
    }

    /// <summary>
    /// Search albums by title
    /// </summary>
    /// <param name="q">Search query</param>
    /// <returns>List of matching albums</returns>
    [HttpGet("search")]
    [ProducesResponseType<IEnumerable<AlbumSummaryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<AlbumSummaryDto>>> SearchAlbums([FromQuery] string q)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                _logger.LogWarning("GET /api/albums/search - Empty search query");
                return BadRequest("Search query cannot be empty");
            }

            _logger.LogInformation("GET /api/albums/search?q={Query} - Searching albums", q);
            var albums = await _albumService.SearchAlbumsAsync(q);
            return Ok(albums);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching albums with query: {Query}", q);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching albums");
        }
    }
}
