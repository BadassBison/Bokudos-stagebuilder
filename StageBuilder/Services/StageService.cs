using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StageBuilder.Database;
using StageBuilder.Models;
using StageBuilder.Dtos;

namespace StageBuilder.Services
{
  public class StageService : IStageService
  {
    public readonly StageBuilderDbContext _context;

    public StageService(StageBuilderDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<StageEntity>> GetAllStagesAsync()
    {
      return await _context.Stages.ToListAsync();
    }

    public async Task<StageEntity> GetStageByIdAsync(int id)
    {
      return await _context.Stages.FirstAsync(m => m.StageId == id);
    }

    public List<StageEntity> GetStagesByName(string name)
    {
      var searchTerm = "%" + name + "%";
      return _context.Stages
        .FromSqlInterpolated($"SELECT * FROM dbo.stages WHERE RTRIM(name) LIKE {searchTerm}")
        .ToList();
    }

    public async Task<StageEntity> AddStageAsync(StageEntity entity)
    {
      var exists = await CheckForStageAsync(entity.Name);
      if (exists) throw new InvalidOperationException($"{entity.Name} already exists");

      _context.Stages.Add(entity);

      if (await _context.SaveChangesAsync() > 0) return entity;
      else throw new Exception("Failed to save to the database");
    }

    public async Task<StageEntity> UpdateStageAsync(StageEntity entity, Stage model)
    {
      entity.Name = model.Name == null ? entity.Name : model.Name;
      entity.Data = model.Data == null ? entity.Data : model.Data;
      entity.UserId = model.UserId == null ? entity.UserId : (int)model.UserId;
      entity.GameId = model.GameId == null ? entity.GameId : (int)model.GameId;
      entity.LastUpdatedDate = DateTime.Now;

      await _context.SaveChangesAsync();
      return entity;
    }

    public async Task<StageEntity> RemoveStageAsync(StageEntity Stage)
    {
      _context.Stages.Remove(Stage);

      if (await _context.SaveChangesAsync() > 0) return Stage;
      else throw new Exception($"Failed to remove {Stage.Name} from the database");
    }

    private async Task<bool> CheckForStageAsync(string name)
    {
      return await _context.Stages.FirstOrDefaultAsync(p => p.Name == name) != null;
    }

  }
}
