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

    public async Task<List<StageEntity>> GetAllPublishedStagesAsync()
    {
      return await _context.Stages
        .Where(s => s.Published == true)
        .ToListAsync();
    }

    public async Task<StageEntity> GetStageByIdAsync(int id)
    {
      return await _context.Stages.FirstAsync(s => s.StageId == id);
    }

    public async Task<List<StageEntity>> GetStagesByName(string name)
    {
      var searchTerm = "%" + name + "%";
      return await _context.Stages
        .FromSqlInterpolated($"SELECT * FROM stages WHERE RTRIM(name) ILIKE {searchTerm}")
        .ToListAsync();
    }

    public async Task<List<StageEntity>> GetStagesByUser(int userId)
    {
      return await _context.Stages
        .Where(s => s.UserId == userId)
        .ToListAsync();
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
      entity.UserId = model.UserId == null ? entity.UserId : (int)model.UserId;
      entity.GameId = model.GameId == null ? entity.GameId : (int)model.GameId;
      entity.Published = model.Published == null ? entity.Published : (bool)model.Published;
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
      return await _context.Stages.FirstOrDefaultAsync(s => s.Name == name) != null;
    }

  }
}
