using StageBuilder.Models;

namespace StageBuilder.Dtos
{
  public class Stage
  {
#nullable enable
    public int StageId { get; set; }
    public string? Name { get; set; }
    public int? UserId { get; set; }
    public int? GameId { get; set; }
    public bool? Published { get; set; }


    public static implicit operator Stage(StageEntity entity)
    {
      return new Stage
      {
        StageId = entity.StageId,
        Name = entity.Name,
        UserId = entity.UserId,
        GameId = entity.GameId,
        Published = entity.Published
      };
    }
  }
}
