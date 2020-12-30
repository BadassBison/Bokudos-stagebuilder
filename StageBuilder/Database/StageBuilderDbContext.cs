using Microsoft.EntityFrameworkCore;
using StageBuilder.Models;

namespace StageBuilder.Database
{
  public class StageBuilderDbContext : DbContext
  {
    public StageBuilderDbContext(DbContextOptions<StageBuilderDbContext> options) : base(options) { }
    public DbSet<StageEntity> Stages { get; set; }
  }
}
