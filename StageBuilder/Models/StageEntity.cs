using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StageBuilder.Dtos;

namespace StageBuilder.Models
{
  [Table("stages")]
  public class StageEntity
  {
    [Key]
    [Column("stageId")]
    public int StageId { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("data")]
    public string Data { get; set; }
    [Column("userId")]
    public int UserId { get; set; }
    [Column("gameId")]
    public int GameId { get; set; }
    [Column("createdDate")]
    public DateTime CreatedDate { get; set; }
    [Column("lastUpdatedDate")]
    public DateTime LastUpdatedDate { get; set; }

    public static implicit operator StageEntity(Stage dto)
    {
      return new StageEntity
      {
        StageId = dto.StageId,
        Name = dto.Name,
        Data = dto.Data,
        UserId = (int)dto.UserId,
        GameId = (int)dto.GameId,
        CreatedDate = DateTime.Now,
        LastUpdatedDate = DateTime.Now
      };
    }
  }
}
