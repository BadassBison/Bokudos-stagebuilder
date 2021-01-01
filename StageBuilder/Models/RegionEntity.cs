using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StageBuilder.Dtos;

namespace StageBuilder.Models
{
  [Table("regions")]
  public class RegionEntity
  {
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("stage")]
    [Column("stageId")]
    public int StageId { get; set; }
    public StageEntity Stage { get; set; }

    [Column("row")]
    public int Row { get; set; }

    [Column("column")]
    public int Column { get; set; }

    [Column("data")]
    public string Data { get; set; }

    [Column("createdDate")]
    public DateTime CreatedDate { get; set; }

    [Column("lastUpdatedDate")]
    public DateTime LastUpdatedDate { get; set; }

    public static implicit operator RegionEntity(Region dto)
    {
      return new RegionEntity
      {
        StageId = dto.StageId,
        Row = (int)dto.Row,
        Column = (int)dto.Column,
        Data = dto.Data,
        CreatedDate = DateTime.Now,
        LastUpdatedDate = DateTime.Now
      };
    }
  }
}
