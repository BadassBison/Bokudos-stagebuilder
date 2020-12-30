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
  /// API REST Endpoints for Stages
  /// </summary>
  [ApiController]
  [Route("[controller]")]
  public class StageController : ControllerBase
  {
    private readonly IStageService _service;
    private readonly IHttpContextAccessor _http;
    private readonly IMapper _mapper;
    private readonly ILogger<StageController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="service"></param>
    /// <param name="http"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    public StageController(IStageService service, IHttpContextAccessor http, IMapper mapper, ILogger<StageController> logger)
    {
      _service = service;
      _http = http;
      _mapper = mapper;
      _logger = logger;
    }

    /// <summary>
    /// Fetches all stages
    /// </summary>
    /// <returns>A collection of stages</returns>
    /// <response code="200">OK if it was a successful fetch</response>
    /// <response code="404">Could not find any stages in the database</response>
    /// <response code="500">Database failure</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<Stage>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Stage>>> GetAllStages()
    {
      try
      {
        _logger.LogInformation("Fetching All Stages");

        var stages = await _service.GetAllStagesAsync();
        if (!stages.Any()) return NotFound("No stages were found in the database");

        return _mapper.Map<List<Stage>>(stages);
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    /// <summary>
    /// Fetches a stage with a specific ID
    /// </summary>
    /// <returns>A stage with a specific ID</returns>
    /// <response code="200">OK if it was a successful fetch</response>
    /// <response code="404">Could not find stage with given id</response>
    /// <response code="500">Database failure</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Stage), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Stage>> GetStageById([FromRoute] int id)
    {
      try
      {
        _logger.LogInformation($"Fetching stage with id {id}");

        var stage = await _service.GetStageByIdAsync(id);
        return _mapper.Map<Stage>(stage);
      }
      catch (InvalidOperationException)
      {
        return NotFound($"No stage with id '{id}' is in the database");
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    /// <summary>
    /// Fetches stages with a name similiar to the search term
    /// </summary>
    /// <returns>stages with a name similiar to the search term</returns>
    /// <response code="200">OK if it was a successful fetch</response>
    /// <response code="404">Could not find stage with given id</response>
    /// <response code="500">Database failure</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<Stage>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<Stage>> SearchStageByName(string name)
    {
      try
      {
        _logger.LogInformation($"Fetching stages with name similiar to {name}");

        var stages = _service.GetStagesByName(name);
        if (!stages.Any()) return NotFound($"No stages were found that were similiar to '{name}'");

        return _mapper.Map<List<Stage>>(stages);
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    /// <summary>
    /// Adds a stage
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /stages
    ///     {
    ///        "name": "stageName",
    ///        "userId": 1,
    ///        "gameId": 1,
    ///     }
    ///
    /// </remarks>
    /// <returns>The stage that was added</returns>
    /// <response code="201">OK if it was a successful post</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Database failure</response>
    [HttpPost]
    [ProducesResponseType(typeof(Stage), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Stage>> AddStage([FromBody] Stage dto)
    {
      try
      {
        _logger.LogInformation($"Adding Stage {dto.Name}");

        var stage = _mapper.Map<StageEntity>(dto);
        stage.CreatedDate = DateTime.Now;
        stage.LastUpdatedDate = stage.CreatedDate;

        await _service.AddStageAsync(stage);

        var uri = _http.HttpContext.Request.Host.Value;
        return Created(uri, _mapper.Map<Stage>(stage));
      }
      catch (InvalidOperationException)
      {
        return BadRequest("Stage already exists");
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    /// <summary>
    /// Updates a stage.
    /// You can pass any properties and the whole record will be updated
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /stages/:id
    ///     {
    ///        "name": "stageName",
    ///        "userId": 1,
    ///        "gameId": 1,
    ///     }
    ///
    /// </remarks>
    /// <returns>The stage that was updated</returns>
    /// <response code="200">OK if it was a successful put</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Database failure</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Stage), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Stage>> UpdateStage([FromBody] Stage dto, [FromRoute] int id)
    {
      try
      {
        _logger.LogInformation($"Updating Stage {dto.Name}");

        var stageEntity = await _service.GetStageByIdAsync(id);

        stageEntity = await _service.UpdateStageAsync(stageEntity, dto);
        return _mapper.Map<Stage>(stageEntity);
      }
      catch (InvalidOperationException)
      {
        return BadRequest($"Stage with id {id} or name {dto.Name} does not exist");
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure ");
      }
    }

    /// <summary>
    /// Removes a stage from the database
    /// </summary>
    /// <returns></returns>
    /// <response code="200">OK if it was a successful removal</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Database failure</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Stage>> DeleteStage([FromRoute] int id)
    {
      try
      {
        _logger.LogInformation($"Deleting Stage with id {id}");

        var stage = await _service.GetStageByIdAsync(id);

        await _service.RemoveStageAsync(stage);
        return Ok($"Stage '{stage.Name}' was removed from the database");
      }
      catch (InvalidOperationException)
      {
        return BadRequest($"Stage with id {id} is not in the database to be removed");
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }
  }
}
