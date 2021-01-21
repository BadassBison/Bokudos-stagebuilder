using System;
using System.Collections.Generic;
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

    [Column("userId")]
    public int UserId { get; set; }

    [Column("gameId")]
    public int GameId { get; set; }

    [Column("published")]
    public bool Published { get; set; }

    [Column("topBoundary")]
    public int TopBoundary { get; set; }

    [Column("bottomBoundary")]
    public int BottomBoundary { get; set; }

    [Column("leftBoundary")]
    public int LeftBoundary { get; set; }

    [Column("rightBoundary")]
    public int RightBoundary { get; set; }

    [Column("createdDate")]
    public DateTime CreatedDate { get; set; }

    [Column("lastUpdatedDate")]
    public DateTime LastUpdatedDate { get; set; }

    public List<RegionEntity> Regions { get; set; }

    public static implicit operator StageEntity(Stage dto)
    {
      return new StageEntity
      {
        StageId = dto.StageId,
        Name = dto.Name,
        UserId = (int)dto.UserId,
        GameId = (int)dto.GameId,
        Published = (bool)dto.Published,
        TopBoundary = (int)dto.TopBoundary,
        BottomBoundary = (int)dto.BottomBoundary,
        LeftBoundary = (int)dto.LeftBoundary,
        RightBoundary = (int)dto.RightBoundary,
        CreatedDate = DateTime.Now,
        LastUpdatedDate = DateTime.Now
      };
    }
  }
}
