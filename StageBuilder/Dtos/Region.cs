using StageBuilder.Models;

namespace StageBuilder.Dtos
{
  public class Region
  {
#nullable enable
    public int Id { get; set; }
    public int StageId { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public string? Data { get; set; }


    public static implicit operator Region(RegionEntity entity)
    {
      return new Region
      {
        StageId = entity.StageId,
        Row = entity.Row,
        Column = entity.Column,
        Data = entity.Data
      };
    }
  }
}
