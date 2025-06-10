using Microsoft.AspNetCore.Mvc;
using Metalify.Api.DTOs;
using Metalify.Api.Services;

namespace Metalify.Api.Controllers;

/// <summary>
/// REST API controller for Song operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SongsController : ControllerBase
{
    private readonly ISongService _songService;
    private readonly ILogger<SongsController> _logger;

    public SongsController(ISongService songService, ILogger<SongsController> logger)
    {
        _songService = songService ?? throw new ArgumentNullException(nameof(songService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all songs
    /// </summary>
    /// <returns>List of all songs</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<SongDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetAllSongs()
    {
        try
        {
            _logger.LogInformation("GET /api/songs - Retrieving all songs");
            var songs = await _songService.GetAllSongsAsync();
            return Ok(songs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all songs");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving songs");
        }
    }

    /// <summary>
    /// Get song by ID
    /// </summary>
    /// <param name="id">Song ID</param>
    /// <returns>Song details</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<SongDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SongDto>> GetSongById(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("GET /api/songs/{Id} - Invalid song ID: {Id}", id);
                return BadRequest("Song ID cannot be empty");
            }

            _logger.LogInformation("GET /api/songs/{Id} - Retrieving song", id);
            var song = await _songService.GetSongByIdAsync(id);
            
            if (song == null)
            {
                _logger.LogInformation("Song not found with ID: {Id}", id);
                return NotFound($"Song with ID {id} not found");
            }

            return Ok(song);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving song with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the song");
        }
    }

    /// <summary>
    /// Get songs by album
    /// </summary>
    /// <param name="albumId">Album ID</param>
    /// <returns>List of songs in the specified album</returns>
    [HttpGet("album/{albumId:guid}")]
    [ProducesResponseType<IEnumerable<SongDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetSongsByAlbum(Guid albumId)
    {
        try
        {
            if (albumId == Guid.Empty)
            {
                _logger.LogWarning("GET /api/songs/album/{AlbumId} - Invalid album ID: {AlbumId}", albumId);
                return BadRequest("Album ID cannot be empty");
            }

            _logger.LogInformation("GET /api/songs/album/{AlbumId} - Retrieving songs by album", albumId);
            var songs = await _songService.GetSongsByAlbumIdAsync(albumId);
            return Ok(songs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving songs by album ID: {AlbumId}", albumId);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving songs by album");
        }
    }

    /// <summary>
    /// Get songs by artist
    /// </summary>
    /// <param name="artistId">Artist ID</param>
    /// <returns>List of songs by the specified artist</returns>
    [HttpGet("artist/{artistId:guid}")]
    [ProducesResponseType<IEnumerable<SongDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetSongsByArtist(Guid artistId)
    {
        try
        {
            if (artistId == Guid.Empty)
            {
                _logger.LogWarning("GET /api/songs/artist/{ArtistId} - Invalid artist ID: {ArtistId}", artistId);
                return BadRequest("Artist ID cannot be empty");
            }

            _logger.LogInformation("GET /api/songs/artist/{ArtistId} - Retrieving songs by artist", artistId);
            var songs = await _songService.GetSongsByArtistIdAsync(artistId);
            return Ok(songs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving songs by artist ID: {ArtistId}", artistId);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving songs by artist");
        }
    }

    /// <summary>
    /// Search songs by title
    /// </summary>
    /// <param name="q">Search query</param>
    /// <returns>List of matching songs</returns>
    [HttpGet("search")]
    [ProducesResponseType<IEnumerable<SongDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SongDto>>> SearchSongs([FromQuery] string q)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                _logger.LogWarning("GET /api/songs/search - Empty search query");
                return BadRequest("Search query cannot be empty");
            }

            _logger.LogInformation("GET /api/songs/search?q={Query} - Searching songs", q);
            var songs = await _songService.SearchSongsAsync(q);
            return Ok(songs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching songs with query: {Query}", q);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching songs");
        }
    }
}
