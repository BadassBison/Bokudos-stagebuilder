using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StageBuilder.Dtos;
using StageBuilder.Models;
using StageBuilder.Services;

namespace StageBuilder.Controllers
{
  public interface IStageController
  {
    Task<ActionResult<List<Stage>>> GetAllPublishedStages();
    Task<ActionResult<List<Stage>>> GetAllStagesForUser([FromRoute] int userId);
    Task<ActionResult<List<Stage>>> SearchStageByName([FromQuery] string name);
    Task<ActionResult<Stage>> GetStageById([FromRoute] int id);
    Task<ActionResult<Stage>> AddStage([FromBody] Stage dto);
    Task<ActionResult<Stage>> UpdateStage([FromBody] Stage dto, [FromRoute] int id);
    Task<ActionResult<Stage>> DeleteStage([FromRoute] int id);
  }
}
