using System.Collections.Generic;
using System.Threading.Tasks;
using StageBuilder.Dtos;
using StageBuilder.Models;

namespace StageBuilder.Services
{
  public interface IStageService
  {
    Task<List<StageEntity>> GetAllPublishedStagesAsync();
    Task<StageEntity> GetStageByIdAsync(int id);
    Task<List<StageEntity>> GetStagesByName(string name);
    Task<List<StageEntity>> GetStagesByUser(int userId);
    Task<StageEntity> AddStageAsync(StageEntity entity);
    Task<StageEntity> UpdateStageAsync(StageEntity entity, Stage model);
    Task<StageEntity> RemoveStageAsync(StageEntity stage);
  }
}
