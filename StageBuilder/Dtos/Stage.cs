using StageBuilder.Models;

namespace StageBuilder.Dtos
{
  public class Stage
  {
#nullable enable
    public int StageId { get; set; }
    public string? Name { get; set; }
    public string? Data { get; set; }
    public int? UserId { get; set; }
    public int? GameId { get; set; }


    public static implicit operator Stage(StageEntity entity)
    {
      return new Stage
      {
        StageId = entity.StageId,
        Name = entity.Name,
        Data = entity.Data,
        UserId = entity.UserId,
        GameId = entity.GameId,
      };
    }
  }
}
