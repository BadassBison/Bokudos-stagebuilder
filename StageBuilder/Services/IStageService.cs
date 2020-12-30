using System.Collections.Generic;
using System.Threading.Tasks;
using StageBuilder.Dtos;
using StageBuilder.Models;

namespace StageBuilder.Services
{
  public interface IStageService
  {
    Task<IEnumerable<StageEntity>> GetAllStagesAsync();
    Task<StageEntity> GetStageByIdAsync(int id);
    List<StageEntity> GetStagesByName(string name);
    Task<StageEntity> AddStageAsync(StageEntity entity);
    Task<StageEntity> UpdateStageAsync(StageEntity entity, Stage model);
    Task<StageEntity> RemoveStageAsync(StageEntity stage);
  }
}
