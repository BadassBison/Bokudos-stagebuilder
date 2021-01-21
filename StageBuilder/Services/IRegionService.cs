using System.Collections.Generic;
using System.Threading.Tasks;
using StageBuilder.Dtos;
using StageBuilder.Models;

namespace StageBuilder.Services
{
  public interface IRegionService
  {
    Task<List<RegionEntity>> GetAllRegionsAsync();
    Task<List<RegionEntity>> GetAllRegionsForStageAsync(int stageId);
    Task<RegionEntity> GetRegionByRowAndColumnAsync(int stageId, int row, int column);
    Task<List<RegionEntity>> GetRegionNeighborsAsync(int stageId, int row, int column);
    Task<RegionEntity> AddOrUpdateRegionAsync(Region dto);
  }
}
