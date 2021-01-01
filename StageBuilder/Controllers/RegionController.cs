using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AutoMapper;

using StageBuilder.Dtos;
using StageBuilder.Services;
using StageBuilder.Models;

namespace StageBuilder.Controllers
{
  /// <summary>
  /// API REST Endpoints for Regions
  /// </summary>
  [ApiController]
  [Route("[controller]")]
  public class RegionController : ControllerBase
  {
    private readonly IRegionService _service;
    private readonly IHttpContextAccessor _http;
    private readonly IMapper _mapper;
    private readonly ILogger<RegionController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="service"></param>
    /// <param name="http"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    public RegionController(IRegionService service, IHttpContextAccessor http, IMapper mapper, ILogger<RegionController> logger)
    {
      _service = service;
      _http = http;
      _mapper = mapper;
      _logger = logger;
    }

    /// <summary>
    /// Fetches all regions
    /// </summary>
    /// <returns>A collection of regions</returns>
    /// <response code="200">OK if it was a successful fetch</response>
    /// <response code="404">Could not find any regions in the database</response>
    /// <response code="500">Database failure</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<Region>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Region>>> GetAllRegions()
    {
      try
      {
        _logger.LogInformation("Fetching All Regions");

        var regions = await _service.GetAllRegionsAsync();
        if (!regions.Any()) return NotFound("No regions were found in the database");

        return _mapper.Map<List<Region>>(regions);
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    /// <summary>
    /// Fetches all regions for a stage
    /// </summary>
    /// <returns>A collection of regions</returns>
    /// <response code="200">OK if it was a successful fetch</response>
    /// <response code="404">Could not find any regions in the database</response>
    /// <response code="500">Database failure</response>
    [HttpGet("{stageId:int}")]
    [ProducesResponseType(typeof(List<Region>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Region>>> GetAllRegionsForStage([FromRoute] int stageId)
    {
      try
      {
        _logger.LogInformation($"Fetching All Regions for stageId {stageId}");

        var regions = await _service.GetAllRegionsForStageAsync(stageId);
        if (!regions.Any()) return NotFound("No regions were found in the database");

        return _mapper.Map<List<Region>>(regions);
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    /// <summary>
    /// Fetches a region by row and column
    /// </summary>
    /// <returns>A region</returns>
    /// <response code="200">OK if it was a successful fetch</response>
    /// <response code="404">Could not find any regions in the database</response>
    /// <response code="500">Database failure</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(Region), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Region>> GetRegionByRowAndColumn(int row, int column)
    {
      try
      {
        _logger.LogInformation("Fetching Region");

        var region = await _service.GetRegionByRowAndColumnAsync(row, column);
        if (region == null) return NotFound($"No regions were found in the database that have row {row} and column {column}");

        return _mapper.Map<Region>(region);
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    /// <summary>
    /// Adds a region
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /region
    ///     {
    ///        "stageId": 1,
    ///        "row": 1,
    ///        "column": 0,
    ///        "data": "1,1,1n2,2,2n3,3,3"
    ///     }
    ///
    /// </remarks>
    /// <returns>The region that was added</returns>
    /// <response code="201">OK if it was a successful post</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Database failure</response>
    [HttpPost]
    [ProducesResponseType(typeof(Region), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Region>> AddRegion([FromBody] Region dto)
    {
      try
      {
        _logger.LogInformation($"Adding Region for stageId {dto.StageId}");

        var region = await _service.AddOrUpdateRegionAsync(dto);

        var uri = _http.HttpContext.Request.Host.Value;
        return Created(uri, _mapper.Map<Region>(region));
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }
  }
}